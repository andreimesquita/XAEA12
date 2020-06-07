using System;
using Sources.Common.Pattern;
using Sources.Photon;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Views
{
    public sealed class LobbyPatternView : MonoBehaviour
    {
        [SerializeField]
        private Image[] _coloredSlots = new Image[4];
        
        private void Start()
        {
            var photonFacade = PhotonFacade.Instance;
            OnPatternChanged(photonFacade.CurrentPattern);
            photonFacade.OnPatternChanged += OnPatternChanged;
        }

        private void OnDestroy()
        {
            var photonFacade = PhotonFacade.Instance;
            photonFacade.OnPatternChanged -= OnPatternChanged;
        }

        private void OnPatternChanged(int pattern)
        {
            PatternMapperSO mapper = PatternMapperSO.Instance;
            for (int i = 0; i < 4; i++)
            {
                Image slot = _coloredSlots[i];
                byte colorPattern = ColorPatternHelper.ToSinglePattern(pattern, i);
                slot.color = mapper.GetColorByPattern(colorPattern);
            }
        }
    }
}
