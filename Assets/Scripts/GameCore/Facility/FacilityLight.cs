using UnityEngine;

namespace GameCore.Facility
{
    public class FacilityLight : MonoBehaviour
    {
        public AnimationCurve animationEnable;
        public AnimationCurve animationDisable;
        public int materialId;

        [HideInInspector] public Color colorMaterial;
        [HideInInspector] public Color colorLight;

        public AudioSource mainNoice, distarche;

        [Header("Damage Settings")]
        public bool isDamaged = false;                 // режим повреждения
        public float flickerMinDelay = 2f;          // минимальное время между морганиями
        public float flickerMaxDelay = 3f;           // максимальное время между морганиями
        private float flickerTimer;                    // таймер для случайного моргания

        [Header("Spark Effect")]
        public ParticleSystem sparkEffect;             // сюда можно подкинуть prefab искр
        public bool playSparksOnDisable = false;

        private float remainingFlicker;
        private float curAnimationProgress;
        private MeshRenderer renderer;
        private Light lightSource;
        private Material targetMaterial;
        private Color initialEmissionColor;
        private bool isEnabled;
        private bool warheadEnabled;

        private static readonly int EmissionColorID = Shader.PropertyToID("_EmissiveColor");
        private bool hasRenderer;
        private bool hasLight;

        private void Start()
        {
            lightSource = GetComponentInChildren<Light>();
            renderer = GetComponent<MeshRenderer>();

            hasRenderer = renderer != null;
            hasLight = lightSource != null;

            if (hasRenderer)
            {
                var materials = renderer.materials;
                targetMaterial = new Material(materials[materialId]);
                materials[materialId] = targetMaterial;
                renderer.materials = materials;
                initialEmissionColor = targetMaterial.GetColor(EmissionColorID);
                colorMaterial = initialEmissionColor;
            }

            if (hasLight)
                colorLight = lightSource.color;

            flickerTimer = Random.Range(flickerMinDelay, flickerMaxDelay);
        }

        public void OnWarheadEnable()
        {
            warheadEnabled = true;
            if (hasRenderer)
                targetMaterial.SetColor(EmissionColorID, Color.red * 10);
        }

        public void OnReactorComplete()
        {
            remainingFlicker = 0;
            isEnabled = false;

            if (hasLight)
                lightSource.color = Color.black;

            if (hasRenderer)
                targetMaterial.SetColor(EmissionColorID, Color.black);

            // 💥 Добавляем эффект искр
            if (playSparksOnDisable && sparkEffect != null)
            {
                if (distarche != null)
                    distarche.Play();
                if (mainNoice != null)
                    mainNoice.volume = 0;

                sparkEffect.Play();
            }
        }
        public void OnWarheadDisable()
        {
            warheadEnabled = false;
            if (hasRenderer)
                targetMaterial.SetColor(EmissionColorID, initialEmissionColor);
        }

        private void Update()
        {
            // Если лампа повреждена — запускаем случайное мигание
            if (isDamaged)
            {
                flickerTimer -= Time.deltaTime;
                if (flickerTimer <= 0f)
                {
                    // случайное включение/выключение
                    bool flickerOn = Random.value > 0.5f;
                    if (flickerOn)
                        EnableFlickering(Random.Range(0.05f, 0.2f));
                    else
                        OnReactorComplete(); // имитируем резкое выключение с искрами

                    flickerTimer = Random.Range(flickerMinDelay, flickerMaxDelay);
                }
            }

            // стандартная логика мигания
            if (remainingFlicker > 0f)
            {
                UpdateAnimationState(animationDisable, true);
                remainingFlicker -= Time.deltaTime;
                if (remainingFlicker <= 0f)
                    curAnimationProgress = 0f;
            }
            else if (isEnabled)
            {
                UpdateAnimationState(animationEnable, false);
                if (curAnimationProgress >= 1f)
                    isEnabled = false;
            }
        }

        private void UpdateAnimationState(AnimationCurve curve, bool isFlickering)
        {
            isEnabled = true;
            curAnimationProgress = Mathf.Clamp01(curAnimationProgress + Time.deltaTime);
            float evaluation = curve.Evaluate(curAnimationProgress);
            if (mainNoice != null)
                mainNoice.volume = evaluation;

            if (hasRenderer)
            {
                Color targetColor = warheadEnabled ? Color.red * 10 : colorMaterial;
                targetMaterial.SetColor(EmissionColorID, Color.Lerp(Color.black, targetColor, evaluation));
            }

            if (hasLight)
            {
                Color targetLightColor = warheadEnabled ? Color.red * 2 : colorLight;
                lightSource.color = Color.Lerp(Color.black, targetLightColor, evaluation);
            }
        }

        public bool IsDisabled()
        {
            return curAnimationProgress >= 1f && !isEnabled;
        }

        public bool EnableFlickering(float dur)
        {
            if (remainingFlicker > 0f) return false;
            remainingFlicker = dur;
            curAnimationProgress = 0f;
            return true;
        }

        private void OnDestroy()
        {
            if (targetMaterial != null)
                DestroyImmediate(targetMaterial);
        }
    }
}
