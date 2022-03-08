namespace EasyAbp.AbpHelper.Core.Models
{
    public class PropertyInfo
    {
        public string Type { get; }

        public string Name { get; }

        public string ColumnName { get; }

        public PropertyInfo(string type, string name,string columnName)
        {
            Type = type;
            Name = name;
            ColumnName = columnName;
        }
    }
}