using System.ComponentModel;

namespace FoodApp.Web.Models.Enums
{
    public enum ProductSizes
    {

        [Description("Small")]
        Small = 1,

        [Description("Medum")]
        Medium = 2,

        [Description("Large")]
        Large = 3,

        [Description("Extra Large")]
        ExtraLarge = 4

    }
}
