using Dapper;
using ToDo.Models.Db;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ToDo.DbAccessLibrary.Repositories.SqLite
{
    /// <inheritdoc cref="IToDoTaskRepository"/>
    internal class ToDoTaskRepository : Db.Repository, IToDoTaskRepository
    {
        public ToDoTaskRepository(Func<IDbConnection> GetConnection, Func<IDbTransaction> GetTransaction) : base(GetConnection, GetTransaction) { }

        /// <inheritdoc/>
        public async Task<ToDoTask> Create(string name, int priority, Models.Db.TaskStatus status)
        {
            var parms = new DynamicParameters();
            parms.Add("NAME", name);
            parms.Add("PRIORITY", priority);
            parms.Add("TASKSTATUSID", status.TaskStatusId);

            var results = await LoadData<ToDoTask, dynamic>(sql: @"
                INSERT INTO [ToDoTask]([Name],[Priority],[TaskStatusId]) VALUES (@NAME,@PRIORITY,@TASKSTATUSID) 
                RETURNING [ToDoTaskId],[TaskStatusId],[Priority],[Name];
            ", commandType: CommandType.Text, parameters: parms);

            return results.SingleOrDefault();
        }

        /// <inheritdoc/>
        public async Task<List<ToDoTask>> GetAll(bool includeFinished = false)
        {
            var parms = new DynamicParameters();
            parms.Add("FINISHED", includeFinished ? 1 : 0);

            var results = await LoadData<ToDoTask, dynamic>(sql: @"
                SELECT [ToDoTaskId],[TaskStatusId],[Priority],[Name] FROM [ToDoTask] WHERE ( [TaskStatusId] IN (1,2,3) AND @FINISHED = 1) OR ( [TaskStatusId] IN (1,2) )
            ", commandType: CommandType.Text, parameters: parms);

            return results.Distinct().ToList();
        }

        /// <inheritdoc/>
        public async Task<bool> CheckExists(string name, int toDoTaskId = 0)
        {
            var parms = new DynamicParameters();
            parms.Add("NAME", name);
            parms.Add("TODOTASKID", toDoTaskId);

            var results = await LoadData<int, dynamic>(sql: @"
                SELECT COUNT(*) FROM [ToDoTask] WHERE [Name] = @NAME AND [ToDoTaskId] <> @TODOTASKID;
            ", commandType: CommandType.Text, parameters: parms);

            var result = results.Single();

            return result > 0;
        }

        /// <inheritdoc/>
        public async Task Update(ToDoTask taskToUpdate)
        {
            var parms = new DynamicParameters();
            parms.Add("TODOTASKID", taskToUpdate.ToDoTaskId);
            parms.Add("NAME", taskToUpdate.Name);
            parms.Add("PRIORITY", taskToUpdate.Priority);
            parms.Add("TASKSTATUSID", taskToUpdate.TaskStatusId);

            await SaveData<dynamic>(sql: @"
                UPDATE [ToDoTask]
                SET [Name] = @NAME
                   ,[TaskStatusId] = @TASKSTATUSID
                   ,[Priority] = @PRIORITY
                WHERE [ToDoTaskId] = @TODOTASKID;
            ", commandType: CommandType.Text, parameters: parms);
        }

        /// <inheritdoc/>
        public async Task<ToDoTask> Get(int toDoTaskId)
        {
            var parms = new DynamicParameters();
            parms.Add("TODOTASKID", toDoTaskId);

            var results = await LoadData<ToDoTask, dynamic>(sql: @"
                SELECT [ToDoTaskId],[TaskStatusId],[Priority],[Name] FROM [ToDoTask] WHERE [ToDoTaskId] = @TODOTASKID
            ", commandType: CommandType.Text, parameters: parms);

            return results.Distinct().SingleOrDefault();
        }

        /// <inheritdoc/>
        public async Task Delete(int toDoTaskId)
        {
            var parms = new DynamicParameters();
            parms.Add("TODOTASKID", toDoTaskId);

            await SaveData<dynamic>(sql: @"
                DELETE FROM [ToDoTask] WHERE [ToDoTaskId] = @TODOTASKID
            ", commandType: CommandType.Text, parameters: parms);
        }
    }
}
