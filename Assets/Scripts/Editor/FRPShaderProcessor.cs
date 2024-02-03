using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class FRPShaderProcessor : IPreprocessShaders
{
    private const string LOG_FILE_PATH = "Library/FRP Shader Compilation Result.txt";

    private static readonly List<ShaderKeyword> funnyLitKeywords = new List<ShaderKeyword>()
    {
    };
    private static readonly List<ShaderKeyword> TNTLitKeywords = new List<ShaderKeyword>()
    {
        new ShaderKeyword("_FORWARD_PLUS"),
        new ShaderKeyword("_MAIN_LIGHT_SHADOWS"),
        new ShaderKeyword("_SHADOWS_SOFT"),
        new ShaderKeyword("DOTS_INSTANCING_ON"),
    };
    private static readonly List<ShaderKeyword> TNTUberKeywords = new List<ShaderKeyword>(){
        new ShaderKeyword("_MAIN_LIGHT_SHADOWS"),
        new ShaderKeyword("_MAINTEX_A_ON"),
        new ShaderKeyword("_MAINTEX02_UVOFFSET_ON"),
        new ShaderKeyword("_USE_DEPTH_EDGE"),
        new ShaderKeyword("_USE_MAINTEX02"),
        new ShaderKeyword("DOTS_INSTANCING_ON"),
        new ShaderKeyword("FOG_EXP")
    };

    public int callbackOrder
    {
        get
        {
            return 0;
        }
    }

    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
    {
        //StripFunnyLit(shader, snippet, data);
        //ShaderFilter(shader, snippet, data, "SoFunny/Funnyland/FunnyLit", funnyLitKeywords);
        //ShaderFilter(shader, snippet, data, "SoFunny/TNT/TNTUber", TNTUberKeywords);
        //Debug.Log(shader.name);

    }

    void StripTest(Shader shader, ShaderSnippetData snippetData, IList<ShaderCompilerData> data)
    {
        if (shader.name == "SoFunny/TNT/TNTTEST")
        {
            UnityEngine.Debug.Log("Found SoFunny/TNT/TNTTEST [" + snippetData.shaderType + "] @time | " + System.DateTime.Now.ToString());
            //System.IO.File.Delete(LOG_FILE_PATH);
            System.IO.File.AppendAllText(LOG_FILE_PATH, "\n==== SoFunny/TNT/TNTTEST Varaints: | shader_type = " + snippetData.shaderType + " | " + System.DateTime.Now.ToString());
            //data.Clear();

            for (int i = 0; i < data.Count; i++)
            {
                /*
                if (i > 1)
                {
                    data.RemoveAt(i);
                    continue;
                }
                */

                ShaderKeyword[] shaderKeywords = data[i].shaderKeywordSet.GetShaderKeywords();

                string shaderKeywordsStr = "";

                foreach (ShaderKeyword shaderKeyword in shaderKeywords)
                {
                    shaderKeywordsStr += " ";
                    shaderKeywordsStr += shaderKeyword.name;
                    shaderKeywordsStr += " ";
                }
                System.IO.File.AppendAllText(LOG_FILE_PATH, "\n====\tTNTTest Varaints [" + i + "]: | " + shaderKeywordsStr + " ｜pass_name = " + snippetData.passName + " | pass_type = " + snippetData.passType);

            }
        }
    }

    void StripFunnyLit(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
    {
        UnityEngine.Debug.Log("FRP Processing: " + shader.name + " | " + snippet.shaderType + " | " + snippet.passName);
        if (shader.name == "SoFunny/TNT/TNTLit")
        {
            UnityEngine.Debug.Log("Found TNTLit, Kill it.");
            data.Clear();
        }
    }

    bool MatchKeywordSet(ShaderKeywordSet shaderKeywordSet, List<ShaderKeyword> keywordsMustMatch)
    {
        bool is_Match = true;
        for (int i = 0; i < keywordsMustMatch.Count; i++)
        {
            if (shaderKeywordSet.IsEnabled(keywordsMustMatch[i]))
            {
                continue;
            }
            else
            {
                is_Match = false;
                break;
            }
        }
        return is_Match;
    }

    void ShaderFilter(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data, string shaderName, List<ShaderKeyword> shaderKeywordsToKeep)
    {
        //Debug.Log("building " + shader.name);
        if (shader.name == shaderName)
        {
#if FRP_SHADER_PROCESSOR
            System.IO.File.AppendAllText(LOG_FILE_PATH, "\n\n\n==== | " + shaderName + " VARIANTS: " + System.DateTime.Now.ToString());
#endif
            for (int i = 0; i < data.Count; i++)
            {

                ShaderKeyword[] shaderKeywords = data[i].shaderKeywordSet.GetShaderKeywords();

                string shaderKeywordsStr = "";



                foreach (ShaderKeyword shaderKeyword in shaderKeywords)
                {
                    shaderKeywordsStr += " ";
                    shaderKeywordsStr += shaderKeyword.name;
                    shaderKeywordsStr += " ";
                }

                if (shaderKeywords.Length != shaderKeywordsToKeep.Count && shaderKeywords.Length != 0)
                {
                    data.RemoveAt(i);
                    continue;
                }
                else
                {
                    string undefined = "";
                    if (shaderKeywords.Length == 0)
                    {
                        undefined = snippet.passType + " " + snippet.passName;
                    }
                    else
                    {

                        // totally match
                        //data[i].shaderKeywordSet.IsEnabled(shaderKeywordsToKeep[0]);
                        if (MatchKeywordSet(data[i].shaderKeywordSet, shaderKeywordsToKeep))
                        {
#if FRP_SHADER_PROCESSOR
                            System.IO.File.AppendAllText(LOG_FILE_PATH, "\n==== | " + undefined + shaderKeywordsStr);
#endif
                            continue;
                        }
                        else
                        {
                            data.RemoveAt(i);
                            continue;
                        }


                    }
                    /*
                    #if FRP_SHADER_PROCESSOR
                                        System.IO.File.AppendAllText(LOG_FILE_PATH, "\n==== | " + undefined + shaderKeywordsStr);
                                        //continue;
                    #endif
                    */

                }

            }

        }

    }

}
