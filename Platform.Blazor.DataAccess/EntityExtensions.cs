using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Platform.Common;

namespace Platform.DataAccess
{
  public static class EntityExtensions
  {
    public static IQueryable<T> WithEffYear<T>( this DbSet<T> items, int begEffYear )
      where T : class, IBegEffYearEntity
    {
      return from item in items
        where item.BegEffYear == (
                from sub in items
                where sub.Id == item.Id && sub.BegEffYear <= begEffYear
                select sub.BegEffYear ).Max()
        select item;
    }

    public static IQueryable<T> WithEffDate<T>( this DbSet<T> items, DateTime begEffDate )
      where T : class, IBegEffDateEntity
    {
      return from item in items
        where item.BegEffDate == (
                from sub in items
                where sub.Id == item.Id && sub.BegEffDate <= begEffDate
                select sub.BegEffDate ).Max()
        select item;
    }

    public static IQueryable<T> WithMaxEffDate<T>( this DbSet<T> items )
      where T : class, IBegEffDateEntity
    {
      return from item in items
        where item.BegEffDate == (
                from sub in items
                where sub.Id == item.Id
                select sub.BegEffDate ).Max()
        select item;
    }
  }
}