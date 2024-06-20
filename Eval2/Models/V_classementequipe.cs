using System.ComponentModel.DataAnnotations.Schema;

namespace Eval2.Models
{
    [Table("v_classementequipe")]
    public class V_classementequipe
    {
            public int idequipe { get; set; }
            public string nomequipe { get; set; }
            public int points { get; set; }
            public int rang { get; set; }
        

    }
}
