using System;
using Microsoft.Extensions.DependencyInjection;

namespace ToDo.DbAccessLibrary
{
    /// <summary>
    ///     Sql factory returning new sql connections
    /// </summary>
    public interface ISqlFactory
    {
        /// <summary>
        ///     Connection to ToDo database
        /// </summary>
        IToDoDatabase ToDo { get; }
    }

    /// <inheritdoc cref="Db.ISqlFactory"/>
    public class SqlFactory : ISqlFactory
    {
        private readonly IServiceProvider provider;

        /// <inheritdoc/>
        public IToDoDatabase ToDo { get => provider.GetService<IToDoDatabase>(); }

        public SqlFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }
    }
}
