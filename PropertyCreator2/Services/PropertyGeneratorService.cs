using HelperExtensions;
using PropertyCreator2.Enums;
using PropertyCreator2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
#pragma warning disable CS8604

namespace PropertyCreator2.Services;

public static class PropertyGeneratorService
{
    #region Public Methods

    #region GeneratePropertiesDeclaration
    /// <summary>
    /// Generates the properties complete declaration.
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public static string GeneratePropertiesDeclaration(IList<PropertyData> properties)
    {
        if (!properties.Any())
            return String.Empty;

        StringBuilder propertiesText = new();
        StringBuilder dependencyPropertiesText = new();
        StringBuilder privateVariablesText = new();

        properties.ForEach(_ =>
        {
            propertiesText.AppendLine(_ is DependencyPropertyData dependencyPropertyData
                ? GenerateDependencyProperty(dependencyPropertyData)
                : GenerateSimpleProperty(_ as SimplePropertyData));
            dependencyPropertiesText.Append(GenerateDependencyPropertyRegisterCall(_));
            privateVariablesText.Append(GeneratePrivateVariable(_));
        });

        StringBuilder finalText = new();
        finalText.AppendLines($"#region {(properties.Count == 1 ? "Property" : "Properties")}", "").Append(propertiesText);

        if (dependencyPropertiesText.Length > 0)
            finalText.Append(dependencyPropertiesText);

        finalText.AppendLine("#endregion");

        if (privateVariablesText.Length > 0)
            finalText.AppendLine().Append(privateVariablesText);

        return finalText.ToString();
    }
    #endregion

    #endregion

    #region Private Methods

    #region AppendCategory
    /// <summary>
    /// Appends the category data into the property declaration.
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="category"></param>
    private static void AppendCategory(StringBuilder sb, string category)
    {
        if (!category.IsNullOrWhiteSpace() && category.ToLower() != "none")
            sb.AppendLine("[Category(\"", category, "\")]");
    }
    #endregion

    #region AppendDescription
    /// <summary>
    /// Appends the declaration data into the property declaration.
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="description"></param>
    /// <param name="useAttribute"></param>
    private static void AppendDescription(StringBuilder sb, string description, bool useAttribute)
    {
        if (useAttribute && !description.IsNullOrWhiteSpace())
        {
            if (!description.EndsWith('.'))
                description += '.';

            sb.AppendLine("[Description(\"", description, "\")]");
        }
    }
    #endregion

    #region AppendSummary
    /// <summary>
    /// Appends the summary to the property declaration.
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="summary"></param>
    /// <returns></returns>
    private static void AppendSummary(StringBuilder sb, string summary)
    {
        if (!summary.IsNullOrWhiteSpace())
        {
            if (!summary.EndsWith('.'))
                summary += '.';

            sb.AppendLine("/// <summary>").AppendLine("/// ", summary).AppendLine("/// </summary>");
        }
    }
    #endregion

    #region GenerateDependencyProperty
    /// <summary>
    /// Generates a Dependency Property declaration according to the specified <see cref="DependencyPropertyData"/>.
    /// </summary>
    /// <param name="dependencyPropertyData"></param>
    /// <returns></returns>
    private static string GenerateDependencyProperty(DependencyPropertyData dependencyPropertyData)
    {
        StringBuilder sb = new();

        AppendSummary(sb, dependencyPropertyData.Description);
        AppendCategory(sb, dependencyPropertyData.Category);

        if (IsBasicType(dependencyPropertyData.Type))
            sb.AppendLine("[DefaultValue(", GetDefaultValueText(dependencyPropertyData), ")]");

        else if (!dependencyPropertyData.DefaultValue.IsNullOrWhiteSpace())
            sb.AppendLine(
                "[DefaultValue(typeof(",
                dependencyPropertyData.AttributeDefaultValueType,
                "), \"",
                dependencyPropertyData.AttributeDefaultValue,
                "\")]");

        AppendDescription(sb, dependencyPropertyData.Description, dependencyPropertyData.UseDescriptionAttribute);

        sb.Append(GetAccessModifierText(dependencyPropertyData.AccessModifier));

        if (IsHidingUserControlMember(dependencyPropertyData.Name))
            sb.Append("new ");

        sb.Append(dependencyPropertyData.Type, " ", dependencyPropertyData.Name);

        if (dependencyPropertyData.GetStatus != PropertyMethodStatus.None
            || dependencyPropertyData.SetStatus != PropertyMethodStatus.None)
        {
            sb.AppendLines("", "{");

            if (dependencyPropertyData.GetStatus != PropertyMethodStatus.None)
            {
                sb.Append("\t", GetAccessModifierText(dependencyPropertyData.GetStatus), "get => ");

                if (!dependencyPropertyData.Type.EqualsAny(StringComparison.Ordinal, "object", "Object"))
                    sb.Append("(", dependencyPropertyData.Type, ")");

                sb.AppendLine("GetValue(", dependencyPropertyData.Name, "Property);");
            }

            if (dependencyPropertyData.SetStatus != PropertyMethodStatus.None)
                sb.AppendLine("\t", GetAccessModifierText(dependencyPropertyData.SetStatus), "set => SetValue(",
                    dependencyPropertyData.Name, "Property, value);");

            sb.AppendLine("}");
        }
        else
            sb.AppendLine(" => ", GetDefaultValueText(dependencyPropertyData), ";");

        return sb.ToString();
    }
    #endregion

    #region GenerateDependencyPropertyRegisterCall
    /// <summary>
    /// Generates the Dependency Property Register method call.
    /// </summary>
    /// <param name="propertyData"></param>
    /// <returns></returns>
    private static string GenerateDependencyPropertyRegisterCall(PropertyData propertyData)
    {
        if (propertyData is not DependencyPropertyData dependencyPropertyData)
            return String.Empty;

        StringBuilder sb = new("public ");

        if (IsHidingUserControlMember(dependencyPropertyData.Name))
            sb.Append("new ");

        string options = IsBasicType(dependencyPropertyData.Type)
            ? "FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault"
            : "FrameworkPropertyMetadataOptions.AffectsRender";

        sb.AppendLine("static readonly DependencyProperty ", dependencyPropertyData.Name, "Property =")
            .AppendLine("\tDependencyProperty.Register(")
            .AppendLine("\t\tnameof(", dependencyPropertyData.Name, "),")
            .AppendLine("\t\ttypeof(", dependencyPropertyData.Type, "),")
            .AppendLine("\t\ttypeof(", dependencyPropertyData.OwnerType, "),")
            .AppendLine("\t\tnew FrameworkPropertyMetadata(")
            .AppendLine("\t\t\t", GetDefaultValueText(propertyData), ",")
            .AppendLine("\t\t\t", options, "));")
            .AppendLine();

        return sb.ToString();
    }
    #endregion

    #region GeneratePrivateVariable
    /// <summary>
    /// Generates the private variable of a given property if needed.
    /// </summary>
    /// <param name="propertyData"></param>
    /// <returns></returns>
    private static string GeneratePrivateVariable(PropertyData propertyData)
    {
        if (propertyData is not SimplePropertyData { UsePrivateVariable: true })
            return String.Empty;

        StringBuilder sb = new("private ");
        sb.Append(propertyData.Type).Append(' ').Append(GetPrivateVariableName(propertyData.Name));

        if (!propertyData.DefaultValue.IsNullOrWhiteSpace())
            sb.Append(" = ").Append(GetDefaultValueText(propertyData));

        sb.AppendLine(';');
        return sb.ToString();
    }
    #endregion

    #region GenerateSimpleProperty
    /// <summary>
    /// Generates a simple Property declaration according to the specified <see cref="DependencyPropertyData"/>;.
    /// </summary>
    /// <param name="simplePropertyData"></param>
    /// <returns></returns>
    private static string GenerateSimpleProperty(SimplePropertyData simplePropertyData)
    {
        StringBuilder sb = new();

        AppendSummary(sb, simplePropertyData.Description);
        AppendCategory(sb, simplePropertyData.Category);
        AppendDescription(sb, simplePropertyData.Description, simplePropertyData.UseDescriptionAttribute);

        sb.Append(GetAccessModifierText(simplePropertyData.AccessModifier), simplePropertyData.Type, " ",
            simplePropertyData.Name);

        if (simplePropertyData.GetStatus != PropertyMethodStatus.None
            || simplePropertyData.SetStatus != PropertyMethodStatus.None)
        {
            if (simplePropertyData.UsePrivateVariable)
            {
                string privateVariableName = GetPrivateVariableName(simplePropertyData.Name);

                sb.AppendLines("", "{");

                if (simplePropertyData.GetStatus != PropertyMethodStatus.None)
                    sb.AppendLine("\t", GetAccessModifierText(simplePropertyData.GetStatus), "get => ",
                        privateVariableName, ";");

                if (simplePropertyData.SetStatus != PropertyMethodStatus.None)
                {
                    sb.Append('\t', GetAccessModifierText(simplePropertyData.SetStatus), "set");

                    if (simplePropertyData.NotifyChanges)
                        sb.AppendLines("", "\t{")
                            .AppendLine("\t\t", privateVariableName, " = value;")
                            .AppendLine("\t\tPropertyChanged?.Invoke(this, new(nameof(", simplePropertyData.Name, ")));")
                            .AppendLine("\t}");
                    else
                        sb.AppendLine(" => ", privateVariableName, " = value;");
                }

                sb.AppendLine("}");
            }
            else
            {
                sb.Append(" { ");

                if (simplePropertyData.GetStatus != PropertyMethodStatus.None)
                    sb.Append(GetAccessModifierText(simplePropertyData.GetStatus), "get; ");

                if (simplePropertyData.SetStatus != PropertyMethodStatus.None)
                    sb.Append(GetAccessModifierText(simplePropertyData.SetStatus), "set; ");

                sb.Append('}');

                if (!simplePropertyData.DefaultValue.IsNullOrWhiteSpace())
                    sb.AppendLine(" = ", GetDefaultValueText(simplePropertyData), ";");
                else
                    sb.AppendLine();
            }
        }
        else
            sb.AppendLine(" => ", GetDefaultValueText(simplePropertyData), ";");

        return sb.ToString();
    }
    #endregion

    #region GetAccessModifierText
    /// <summary>
    /// Gets the access modifier according to the specified <see cref="AccessModifier"/>.
    /// </summary>
    /// <param name="accessModifier"></param>
    /// <returns></returns>
    private static string GetAccessModifierText(AccessModifier accessModifier) =>
        accessModifier switch
        {
            AccessModifier.Internal => "internal ",
            AccessModifier.Private => "private ",
            AccessModifier.PrivateProtected => "private protected ",
            AccessModifier.Protected => "protected ",
            AccessModifier.ProtectedInternal => "protected internal ",
            AccessModifier.Public => "public ",
            _ => "public"
        };
    #endregion

    #region GetAccessModifierText
    /// <summary>
    /// Gets the access modifier according to the specified <see cref="PropertyMethodStatus"/>.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    private static string GetAccessModifierText(PropertyMethodStatus status) =>
        status == PropertyMethodStatus.Private ? "private " : "";
    #endregion

    #region GetDefaultValueText
    /// <summary>
    /// Gets the default value text.
    /// </summary>
    /// <param name="propertyData"></param>
    /// <returns></returns>
    private static string GetDefaultValueText(PropertyData propertyData)
    {
        if (propertyData.DefaultValue.IsNullOrWhiteSpace())
            return $"default({propertyData.Type})";

        if (propertyData.Type.EqualsAny(StringComparison.Ordinal, "string", "String"))
            return $"\"{propertyData.DefaultValue}\"";

        if (!Double.TryParse(propertyData.DefaultValue, out _))
            return propertyData.DefaultValue;

        return propertyData.Type switch
        {
            "decimal" or "Decimal" => $"{propertyData.DefaultValue}M",
            "double" or "Double" => $"{propertyData.DefaultValue}D",
            "float" or "Single" => $"{propertyData.DefaultValue}F",
            "long" or "Int64" => $"{propertyData.DefaultValue}L",
            "uint" or "UInt32" => $"{propertyData.DefaultValue}U",
            "ulong" or "UInt64" => $"{propertyData.DefaultValue}UL",
            _ => propertyData.DefaultValue
        };
    }
    #endregion

    #region GetPrivateVariableName
    /// <summary>
    /// Gets the private variable name.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    private static string GetPrivateVariableName(string propertyName) =>
        propertyName.Length == 1
            ? $"_{propertyName.ToLower()}"
            : $"_{Char.ToLower(propertyName[0])}{propertyName[1..]}";
    #endregion

    #region IsBasicType
    /// <summary>
    /// Indicates whether the specified type is a C# basic type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool IsBasicType(string type) =>
        type.EqualsAny(
            StringComparison.Ordinal,
            "bool", "Boolean",
            "byte", "Byte",
            "char", "Char",
            "decimal", "Decimal",
            "double", "Double",
            "float", "Single",
            "int", "Int32",
            "long", "Int64",
            "object", "Object",
            "sbyte", "SByte",
            "short", "Int16",
            "string", "String",
            "uint", "UInt32",
            "ulong", "UInt64",
            "ushort", "UInt16");
    #endregion

    #region IsHidingUserControlMember
    /// <summary>
    /// Gets whether the current property name hides a <see cref="UserControl"/> base member.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private static bool IsHidingUserControlMember(string name) =>
        typeof(UserControl).GetProperty(name) is not null;
    #endregion

    #endregion
}