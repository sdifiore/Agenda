using SQLite;

namespace Agenda
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}

