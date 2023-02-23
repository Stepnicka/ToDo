using ToDo.DbAccessLibrary;

namespace ToDo.Test.Mock
{
    internal class SqlFactory : ISqlFactory
    {
        private readonly MockingDatabase db;

        public IToDoDatabase ToDo => new ToDoDatabase(db);

        public SqlFactory(MockingDatabase db)
        {
            this.db = db;
        }
    }
}
