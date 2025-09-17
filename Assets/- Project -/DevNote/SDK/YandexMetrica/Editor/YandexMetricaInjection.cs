using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DevNote.Services.YandexMetrica
{
    public static class YandexMetricaInjection
    {
        private const string CONFIG_PATH = "Assets/DevNote/SDK/YandexMetrica/YandexMetricaConfig.asset";
        private const string START_MARKER = "<!-- Yandex.Metrika counter -->";


        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.WebGL) return;

            var config = AssetDatabase.LoadAssetAtPath<YandexMetricaConfig>(CONFIG_PATH);
            if (config == null)
            {
                Debug.LogError($"YandexMetrikaConfig doesn't exist: {CONFIG_PATH}");
                return;
            }

            if (config.IncludeInBuild == false) return;


            string indexPath = Path.Combine(pathToBuiltProject, "index.html");
            if (!File.Exists(indexPath))
            {
                Debug.LogError("index.html doesn't exist!");
                return;
            }

            string html = File.ReadAllText(indexPath);
            string script = GetScriptText(config);

            if (!html.Contains(START_MARKER))
            {
                if (html.Contains("</head>"))
                    html = html.Replace("</head>", script + "\n</head>");

                else if (html.Contains("</body>"))
                    html = html.Replace("</body>", script + "\n</body>");

                else html += script;
            }

            File.WriteAllText(indexPath, html);
            Debug.Log($"Yandex Metrica Counter ID {config.YandexMetricaCounterId} success installed in index.html");
        }



        private static string GetScriptText(YandexMetricaConfig config) =>
        $@"
  <!-- Yandex.Metrika counter -->
  <script type=""text/javascript"">
    (function(m,e,t,r,i,k,a){{m[i]=m[i]||function(){{(m[i].a=m[i].a||[]).push(arguments)}};
    m[i].l=1*new Date();
    for (var j = 0; j < document.scripts.length; j++) {{if (document.scripts[j].src === r) {{ return; }}}}
    k=e.createElement(t),a=e.getElementsByTagName(t)[0],k.async=1,k.src=r,a.parentNode.insertBefore(k,a)}}
    )(window, document, ""script"", ""https://mc.yandex.ru/metrika/tag.js"", ""ym"");
  
    ym({config.YandexMetricaCounterId}, ""init"", {{
      clickmap:true,
      trackLinks:true,
      accurateTrackBounce:true
    }});
  </script>
  <noscript><div><img src=""https://mc.yandex.ru/watch/{config.YandexMetricaCounterId}"" style=""position:absolute; left:-9999px;"" alt="""" /></div></noscript>
  <!-- /Yandex.Metrika counter -->
        ";






    }



}


