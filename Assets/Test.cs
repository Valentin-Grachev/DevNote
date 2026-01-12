using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DevNote;
using GamePush;
using UnityEngine;

public class Test : MonoBehaviour
{


    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UploadData(1);

        else if (Input.GetKeyDown(KeyCode.Alpha2))
            UploadData(2);

        else if (Input.GetKeyDown(KeyCode.Alpha3))
            UploadData(3);

        else if (Input.GetKeyDown(KeyCode.L))
        {
            string content = await LoadData(3);
            print($"Success loaded: {content}");
        }
    }


    private void UploadData(int promptId)
    {
        GP_Files.UploadContent(
            content: $"data_for_prompt_{promptId}",
            filename: $"p{GP_Player.GetID()}_w{promptId}.txt",
            tags: $"prompt_{promptId}");
    }


    private async UniTask<string> LoadData(int promptId)
    {
        List<string> fileUrls = new List<string>();
        string tag = $"prompt_{promptId}";

        bool fetchCompleted = false;
        bool fetchSuccess = false;

        GP_Files.Fetch(filter: new FilesFetchFilter()
        {
            limit = 10,
            tags = new string[] { tag },

        }, 
        onFetch: (fileDataList, canLoadMore) =>
        {
            foreach (var fileData in fileDataList)
                fileUrls.Add(fileData.src);

            fetchCompleted = true;
            fetchSuccess = true;
        }, 
        onFetchError: () =>
        {
            fetchCompleted = true;
            fetchSuccess = false;
        });

        await UniTask.WaitUntil(() => fetchCompleted);

        if (fetchSuccess)
        {
            if (fileUrls.Count > 0)
            {
                string loadUrl = fileUrls.GetRandom();

                string loadedContent = null;
                bool loadContentCompleted = false;

                GP_Files.LoadContent(loadUrl,
                onLoadContent: (content) =>
                {
                    loadedContent = content;
                    loadContentCompleted = true;
                },
                onLoadContentError: () => 
                {
                    loadedContent = null;
                    loadContentCompleted = true;
                });

                await UniTask.WaitUntil(() => loadContentCompleted);

                if (loadedContent == null)
                    Debug.LogError($"[Files] Error while load content! Load URL: {loadUrl}");

                return loadedContent;
            }
            else
            {
                Debug.LogError($"[Files] File with tag {tag} doesn't exist!");
                return null;
            }
        }
        else
        {
            Debug.LogError($"[Files] Fetch error! Prompt id: {promptId}");
            return null;
        }

    }


}
