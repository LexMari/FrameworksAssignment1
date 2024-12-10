using System.Text.Json.Serialization;
using ApiServer.Domain.Enums;

namespace ApiServer.Domain.Entities;

public class ApiSetting
{
    /// <summary>
    /// Setting Id
    /// </summary>
    public string Id { get; private set; }
    
    /// <summary>
    /// Description of settings purpose
    /// </summary>
    public string Description { get; private set; }
    
    /// <summary>
    /// Data type for the setting
    /// </summary>
    public ApiSettingType Type { get; private set; }
    
    /// <summary>
    /// Value of setting as string
    /// </summary>
    public string Value { get; private set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; private set; }
    
    [JsonIgnore]
    public DateTime UpdatedAt { get; private set; }
    
    #region Readonly Properties

    [JsonIgnore]
    public int IntegerValue
    {
        get
        {
            int.TryParse(Value, out var valueResult);
            return valueResult;
        }
    }

    [JsonIgnore]
    public decimal DecimalValue
    {
        get
        {
            decimal.TryParse(Value, out var valueResult);
            return valueResult;
        }
    }

    #endregion
    
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="description"></param>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public ApiSetting(
        string id,
        string description,
        ApiSettingType type,
        string value)
    {
        Id = id;
        Type = type;
        Description = description;
        Value = value;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    
    #endregion
    
    #region Mutator methods

    public void Update(
        string description,
        string value)
    {
        Description = description;
        if (ValidateValue(Type, value))
        {
            Value = value;
            UpdatedAt = DateTime.Now;
        }
    }
    
    #endregion
    
    #region Static helper methods

    public static bool ValidateValue(ApiSettingType type, string value)
    {
        switch (type)
        {
            case ApiSettingType.String:
                return true;
            case ApiSettingType.Integer:
                return int.TryParse(value, out int intResult);
            case ApiSettingType.Decimal:
                return decimal.TryParse(value, out decimal decimalResult);
            default:
                return false;
        }
    }
    
    #endregion
}