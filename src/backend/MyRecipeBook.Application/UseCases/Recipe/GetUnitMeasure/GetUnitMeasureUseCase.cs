using MyRecipeBook.Domain.Enum;

namespace MyRecipeBook.Application.UseCases.Recipe.GetUnitMeasure;

public class GetUnitMeasureUseCase : IGetUnitMeasureUseCase
{
    public async Task<IDictionary<string, string>> Execute() => await GetUnitsMeasure();

    private static async Task<IDictionary<string, string>> GetUnitsMeasure()
    {
        var unitMeasuteValues = Enum.GetValues(typeof(UnitMeasure)).Cast<UnitMeasure>();
        var units = unitMeasuteValues.Cast<UnitMeasure>().Select(x => x.ToString()).ToList();

        return GetUniMeasureDictionary(units);
    }

    private static IDictionary<string, string> GetUniMeasureDictionary(List<string> units)
    {
        var unitsDictionary = new Dictionary<string, string>(); 

        foreach (var item in units) 
        {
            var itemIndex = (units.IndexOf(item) + 1).ToString();    

            unitsDictionary.Add(itemIndex, item);
        }

        return unitsDictionary;
    }
}
