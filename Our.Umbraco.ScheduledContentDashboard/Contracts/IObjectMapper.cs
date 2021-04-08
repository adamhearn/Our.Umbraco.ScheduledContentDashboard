//-----------------------------------------------------------------------------
// 2021 Our Umbraco
//-----------------------------------------------------------------------------

namespace Our.Umbraco.ScheduledContentDashboard.Contracts
{
    /// <summary>
    /// Declaration of an object mapper contract
    /// </summary>
    /// <typeparam name="TFrom">From type</typeparam>
    /// <typeparam name="TTo">To type</typeparam>
    public interface IObjectMapper<TFrom, TTo>
    {
        /// <summary>
        /// Map from one instance of an object to another
        /// </summary>
        /// <param name="from">Object instance to convert from</param>
        /// <returns>Mapped object</returns>
        TTo Map( TFrom from );
    }
}
