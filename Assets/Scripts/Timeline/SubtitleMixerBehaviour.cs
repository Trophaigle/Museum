using TMPro;
using UnityEngine;
using UnityEngine.Playables;


public class SubtitleMixerBehaviour : PlayableBehaviour
{
    private TextMeshProUGUI textComponent;
    private Color originalColor;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        textComponent = playerData as TextMeshProUGUI;

        if (textComponent == null)
            return;

        if (originalColor == default)
            originalColor = textComponent.color;

        int inputCount = playable.GetInputCount();

        float totalWeight = 0f;
        string currentText = "";
       // TMP_FontAsset currentFont = null;
        float alpha = 0f;

        for (int i = 0; i < inputCount; i++)
        {
            float weight = playable.GetInputWeight(i);

            if (weight > 0f)
            {
                var inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);
                var input = inputPlayable.GetBehaviour();

                double time = inputPlayable.GetTime();
                double duration = inputPlayable.GetDuration();

                float fade = 1f;

                // Fade IN
                if (input.fadeIn > 0 && time < input.fadeIn)
                {
                    fade = (float)(time / input.fadeIn);
                }
                // Fade OUT
                else if (input.fadeOut > 0 && time > duration - input.fadeOut)
                {
                    fade = (float)((duration - time) / input.fadeOut);
                }

                currentText = input.text;
                //currentFont = input.font;

                alpha = fade * weight;
                totalWeight += weight;
            }
        }

        // Appliquer texte
        textComponent.text = currentText;

        // Appliquer police
        //if (currentFont != null)
        //    textComponent.font = currentFont;

        // Appliquer alpha (fade)
        Color c = originalColor;
        c.a = alpha;
        textComponent.color = c;
    }
}
