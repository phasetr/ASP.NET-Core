using OpenAI_API;

var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var api = new OpenAIAPI(apiKey);
var result = await api.Completions.GetCompletion("楕円型非線型偏微分方程式の応用に関して教えてください。");
Console.WriteLine(result.Trim());
