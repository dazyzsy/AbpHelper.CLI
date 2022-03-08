using System.Linq;

namespace EasyAbp.AbpHelper.Core.Models
{
    public class ProjectInfo
    {
        public ProjectInfo(string baseDirectory, string baseUiDirectory, string fullName, TemplateType templateType, UiFramework uiFramework, bool tiered)
        {
            BaseDirectory = baseDirectory;
            BaseUiDirectory = baseUiDirectory;
            TemplateType = templateType;
            UiFramework = uiFramework;
            Tiered = tiered;
            FullName = fullName;
        }

        public string BaseDirectory { get; }
        public string BaseUiDirectory { get; }
        public string FullName { get; }
        public string Name => FullName.Split('.').Last();
        public TemplateType TemplateType { get; }
        public UiFramework UiFramework { get; }
        public bool Tiered { get; }

        public override string ToString()
        {
            return $"{nameof(BaseDirectory)}: {BaseDirectory}, {nameof(BaseUiDirectory)}: { BaseUiDirectory},{nameof(FullName)}: {FullName}, {nameof(Name)}: {Name}, {nameof(TemplateType)}: {TemplateType}, {nameof(UiFramework)}: {UiFramework}, {nameof(Tiered)}: {Tiered}";
        }
    }

    public enum TemplateType
    {
        Application,
        Module,
        Test
    }

    public enum UiFramework
    {
        None,
        RazorPages,
        Angular,
        Antd
    }
}