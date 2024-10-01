using SQLite;

namespace qlhui.app.Data.DataAccess.Entities
{
    public class BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NgayTao { get; set; }
    }
}
