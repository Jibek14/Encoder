using System.Data.Linq.Mapping;
namespace LightYagami
{
    [Table(Name ="UserCryptTexts")]
    class CiferText
    {
       [Column(IsPrimaryKey =true,IsDbGenerated =true,Name ="id")]
        public int Id { get; set; }
        [Column(Name = "key")]
        public int Key { get; set; }
        [Column(Name ="text")]
        public string Text { get; set; }
        [Column(Name ="onBaseSave")]
        public int SavePlaceState { get; set; }
    }
}