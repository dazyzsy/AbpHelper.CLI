using {{ EntityInfo.Namespace}};
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaanApiNext.Configurations.TempTransferOrder
{
    public class {{EntityInfo.Name}}Configuration : IEntityTypeConfiguration<{{EntityInfo.Name}}>
    {
        public void Configure(EntityTypeBuilder<{{EntityInfo.Name}}> builder)
        {
            // builder.ToTable("tzzwms008210");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("t_scid");
            builder.Property(t => t.LogisticCompanyNum).HasColumnName("t_lcom");
            builder.Property(t => t.FinanceCompanyNum).HasColumnName("t_fcom");


            {{- for prop in EntityInfo.Properties -}}
            {{- if for.first; "\r\n"; else; "\r\n"; end ~}}
            builder.Property(t => t.{{ prop.Name }}).HasColumnName("{{ prop.ColumnName }}");
            {{- if for.last; "\r\n"; else; ; end -}}
            {{- end ~}}

            builder.Property(t => t.t_Refcntd).HasColumnName("t_Refcntd");
            builder.Property(t => t.t_Refcntu).HasColumnName("t_Refcntu");
        }
    }
}