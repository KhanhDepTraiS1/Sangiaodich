using System.ComponentModel.DataAnnotations;

namespace Sangiaodich.Model
{

    public class MCK
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MCKName { get; set; }
        public string SGD { get; set; }

        public string Price_Tran { get; set; }
        public string Price_San { get; set; }
        public string Price_TC { get; set; }

        public string DM_Price3 { get; set; }
        public string DM_KL3 { get; set; }


        public string DM_Price2 { get; set; }
        public string DM_KL2 { get; set; }

        public string DM_Price1 { get; set; }
        public string DM_KL1 { get; set; }


        public string KL_Price { get; set; }
        public string KL_Updown { get; set; }
        public string KL_KL { get; set; }
        public string KL_KLGD { get; set; }


        public string DB_Price1 { get; set; }
        public string DB_KL1 { get; set; }
        public string DB_Price2 { get; set; }
        public string DB_KL2 { get; set; }
        public string DB_Price3 { get; set; }
        public string DB_KL3 { get; set; }


        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string TB { get; set; }


        public string GDNDTNN_Buy { get; set; }
        public string GDNDTNN_Sell { get; set; }


        public string RoomNN { get; set; }

        public string CL { get; set; }

        public string KLTT { get; set; }
    }
}
