using System.Collections.Generic;
using Newtonsoft.Json;
public class ValueProvider
{
    // Instance Variables
    String? valueFile;
    List<Dictionary<String, dynamic>> parameterList;
    Dictionary<String, String> valuesDict;

    public ValueProvider (String? additonalValueFile= null, Boolean acceptUserInput= false)
    {
        this.valueFile = additonalValueFile;
        this.valuesDict = new Dictionary<String, String>();
        Boolean parsedValues =  this.valueControl();
        if (parsedValues)
        {
            Console.WriteLine("values parsed successfully");
            foreach (KeyValuePair<String, String> item in this.valuesDict )
            {
                Console.WriteLine(item.Key +" : "+ item.Value);
            } 
        }
    }
    internal Boolean valueControl()
    {
        Boolean parseState = true;
        parseState = this.parameterParser();
        if  (!parseState is true) {
            return parseState;
           }
        parseState = this.valueParser();
        if  (!parseState is true) {
            return parseState;
           }
        parseState = this.additionalValueParser();
        if  (!parseState is true) {
            return parseState;
           }
        parseState = this.userInputParser();
        if  (!parseState is true) {
            return parseState;
           }
        parseState = this.consolidateValues();
        if  (!parseState is true) {
            return parseState;
           }
        return parseState;
    }

    internal Boolean parameterParser()
    {
        using ( StreamReader r = new StreamReader("valuecontrol/parameters_config.json"))
        {
          String paramConfig = r.ReadToEnd();
          this.parameterList = JsonConvert.DeserializeObject<List<Dictionary<String, dynamic>>>(paramConfig);
        //   foreach ( Dictionary<String, String> i in this.parameterList  )
        //   {
        //     foreach (KeyValuePair<String, String> item in i )
        //     {
        //         Console.WriteLine(item.Key +" : "+ item.Value);
        //     } 
        //   }
        }
        
        return true;
    }


    internal Boolean valueParser()
    {

        using ( StreamReader r = new StreamReader("valuecontrol/values.json"))
        {
          String paramConfig = r.ReadToEnd();
          Dictionary<String, dynamic> tempValuesDict = new Dictionary<String, dynamic>();
          tempValuesDict = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(paramConfig);
          if (tempValuesDict is null){
            tempValuesDict = new Dictionary<String, dynamic>();
          }
          foreach ( Dictionary<String, dynamic> i in this.parameterList  )
          {
            if (tempValuesDict.ContainsKey(i["name"])){
                i["value"] = (string)tempValuesDict[i["name"]];
            }
          }
        }

        return true;
    }


    internal Boolean additionalValueParser()
    {
        if (this.valueFile is null){
            return true;
        }
        using ( StreamReader r = new StreamReader(this.valueFile))
        {
          String paramConfig = r.ReadToEnd();
          Dictionary<String, dynamic> tempValuesDict = new Dictionary<String, dynamic>();
          tempValuesDict = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(paramConfig);
          if (tempValuesDict is null){
            tempValuesDict = new Dictionary<String, dynamic>();
          }
          foreach ( Dictionary<String, dynamic> i in this.parameterList  )
          {
            if (tempValuesDict.ContainsKey(i["name"])){
                i["value"] = (string)tempValuesDict[i["name"]];
            }
          }
        }

        return true;
    }


    internal Boolean userInputParser()
    {
        return true;
    }


    internal Boolean consolidateValues()
    {
     foreach ( Dictionary<String, dynamic> i in this.parameterList  )
          {
            if (i["value"] is null && i["required"] )
            {
                Console.WriteLine("Error: all required values where not available");
                return false;
            }
            this.valuesDict.Add(i["name"], i["value"]);
          }

        return true;
    }


    public Dictionary<String, String> getValues()
    {
        return this.valuesDict;
    }
}
