using qlhui.app.Data.DataAccess.Entities;
using SQLite;

namespace QlHui.App.Data.Services
{
    internal class BaseService
    {
        protected string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "qlHui.db3");
        protected SQLiteConnection _connection;

        protected void SetUpDd()
        {
            _connection ??= new SQLiteConnection(dbPath);
            if (_connection != null)
            {
                _connection.CreateTable<HuiVienEntity>();
                _connection.CreateTable<HuiVienThamGiaEntity>();
                _connection.CreateTable<LichSuBoHuiEntity>();
                _connection.CreateTable<DayHuiEntity>();
                _connection.CreateTable<LichSuDongHuiEntity>();
            }
        }
        public void DeleteAllTable()
        {
            _connection.DeleteAll<HuiVienEntity>();
            _connection.DeleteAll<HuiVienThamGiaEntity>();
            _connection.DeleteAll<LichSuBoHuiEntity>();
            _connection.DeleteAll<DayHuiEntity>();
            _connection.DeleteAll<LichSuDongHuiEntity>();
        }
        public BaseService()
        {
            SetUpDd();
        }
    }
}
