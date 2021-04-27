using FunctionApp2.Models;
using System.Collections.Generic;

namespace FunctionApp2.QueryInterfaces
{
    public interface ISidekickSqlQueries
    {
        TohSidekick addSidekick(TohSidekick sidekick);

        void deleteSidekick(int id);

        List<TohSidekick> getHeroSidekicks(int id);

        TohSidekick getSidekick(int id);

        List<TohSidekick> getSidekicks();

        TohSidekick updateSidekick(int id, TohSidekick updated);
    }
}