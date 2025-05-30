using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComputerUIManager : MonoBehaviour, IPointerDownHandler
{
    public GameObject taskButtonPrefab;
    public Transform contentParent;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI sender;
    public TextMeshProUGUI subject;
    private Dictionary<GameObject, Task> mails = new Dictionary<GameObject, Task>();
    public RectTransform canvas;
    public TextMeshProUGUI TIME;
    public GameObject SideQuestNone;
    public GameObject SideQuest;
    public TextMeshProUGUI SideTitle;
    public TextMeshProUGUI SideDescription;
    public Task selected;
    public GameObject button;
    void Start()
    {
        int currentEntry = -1;
        foreach (var task in TaskDatabase.All)
        {
            currentEntry++;
            GameObject entry = Instantiate(taskButtonPrefab, contentParent);
            entry.transform.position = new Vector3(taskButtonPrefab.transform.position.x, taskButtonPrefab.transform.position.y - (currentEntry * transform.lossyScale.y * 140), 0);
            entry.GetComponent<TaskEntryUI>().Setup(task, this);
            entry.GetComponent<Image>().color = new Color((float)180 / 255, (float)180 / 255, (float)180 / 255);
            mails.Add(entry, task);
            if (currentEntry == 0)
            {
                entry.GetComponent<Image>().color = Color.white;
                descriptionText.text = task.mailContent;
                sender.text = task.sendermail;
                subject.text = task.subject;
                selected = task;
                // Debug.log(.*)
            }
        }
        taskButtonPrefab.transform.position = new Vector3(taskButtonPrefab.transform.position.x + 10000, taskButtonPrefab.transform.position.y, 0);

        if (TaskDatabase.currentTask == null)
        {
            SideQuest.transform.position = new Vector3(SideQuest.transform.position.x + 10000, SideQuest.transform.position.y, 0);
        }
        else
        {
            button.GetComponent<Image>().color = new Color((float)171 / 255, (float)171 / 255, (float)171 / 255);
            SideDescription.text = TaskDatabase.currentTask.description;
            SideTitle.text = TaskDatabase.currentTask.subject;
            SideQuestNone.transform.position = new Vector3(SideQuestNone.transform.position.x + 10000, 0, 0);
        }
        TIME.text = System.DateTime.Now.ToString();
    }

    private void Update()
    {
        TIME.text = System.DateTime.Now.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (mails.ContainsKey(eventData.pointerCurrentRaycast.gameObject))
        {
            foreach (var mail in mails)
            {
                if (mail.Key == eventData.pointerCurrentRaycast.gameObject)
                {
                    Task t = mail.Value;
                    descriptionText.text = t.mailContent;
                    sender.text = t.sendermail;
                    subject.text = t.subject;
                    mail.Key.GetComponent<Image>().color = Color.white;
                    selected = t;
                }
                else
                {

                    mail.Key.GetComponent<Image>().color = new Color((float)180 / 255, (float)180 / 255, (float)180 / 255);
                }
            }
        }
        else if (mails.ContainsKey(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject))
        {
            GameObject mail = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
            if (mails.TryGetValue(mail, out Task task))
            {
                Task t = task;
                descriptionText.text = t.mailContent;
                sender.text = t.sendermail;
                subject.text = t.subject;
                selected = t;
                foreach (var allmail in mails)
                {
                    allmail.Key.GetComponent<Image>().color = new Color((float)180 / 255, (float)180 / 255, (float)180 / 255);
                }
                mail.GetComponent<Image>().color = Color.white;
            }

        }
    }
}
