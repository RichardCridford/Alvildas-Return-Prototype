using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTrainSound : SoundEmitterClickable
{   
    //Sound object found on Train Carts 
    #region Public Functions 

    protected override void OnClicked()
    {
        audioController.Play();

        LevelController.SoundWave.CreateWave
        (
            new SoundWaveParameters()
            {
                //Wave Type set in GameEnum.cs
                type = WaveType.Traincart,

                origin = transform.position,
                rotation = transform.rotation.eulerAngles.y,
                color = waveColor,
                //set in GameConst.cs
                degrees = GameConst.BLAST_DEGREES,

                strength = waveStrength,
                attractionFactor = attractionStrength
            },
            gameObject
        );
    }

    #endregion
}
