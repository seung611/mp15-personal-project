using UnityEngine;
using UnityEngine.UI;

// root 아래의 모든 버튼(꺼진 것 포함)에 클릭 효과음을 연결한다.
// 버튼마다 인스펙터에서 연결하지 않아도 되고, 새 버튼을 추가해도 자동으로 소리가 난다.
public static class UIClickSound
{
    private const string SKILL_BUTTON_METHOD = "OnClickSkillButton";

    public static void Bind(Transform root)
    {
        foreach (Button button in root.GetComponentsInChildren<Button>(true))
        {
            if (HasOwnSound(button)) continue;
            button.onClick.AddListener(Play);
        }
    }

    private static void Play()
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.PlaySfx(ESfxType.ButtonClick);
    }

    // 이미 자기 소리를 내는 버튼은 건너뛴다 (소리가 두 번 겹치지 않게)
    private static bool HasOwnSound(Button button)
    {
        // ButtonFeedback은 클릭음을 직접 재생한다
        if (button.GetComponent<ButtonFeedback>() != null) return true;

        // 스킬 노드는 구매 성공 / 실패 소리를 따로 낸다
        for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
        {
            if (button.onClick.GetPersistentMethodName(i) == SKILL_BUTTON_METHOD) return true;
        }
        return false;
    }
}
