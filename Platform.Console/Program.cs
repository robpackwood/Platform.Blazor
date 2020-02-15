using System.Linq;
using Platform.Blazor.DataAccess;
using Platform.Blazor.Model;

namespace Platform.Console
{
  internal class Program
  {
    private static void Main()
    {
      var dataContext = new DataContext();

      RevenueObject revObj = dataContext.RevenueObjects.Single(x => x.Id == 11);

      System.Console.WriteLine(revObj.Pin);
    }
  }
}