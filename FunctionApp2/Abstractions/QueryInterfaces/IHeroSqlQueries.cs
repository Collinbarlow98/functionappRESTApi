using FunctionApp2.Models;
using System.Collections.Generic;

namespace FunctionApp2.QueryInterfaces
{
    public interface IHeroSqlQueries
    {
        TohHero addHero(TohHero hero);

        void deleteHero(int id);

        TohHero getHero(int id);

        List<TohHero> getHeroes();

        TohHero updateHero(int id, TohHero updated);
    }
}