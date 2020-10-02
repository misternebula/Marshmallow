﻿using OWML.ModHelper.Events;
using UnityEngine;
using Logger = Marshmallow.Utility.Logger;

namespace Marshmallow.Atmosphere
{
    static class AirBuilder
    {
        public static void Make(GameObject body, float airScale, bool isRaining)
        {
            GameObject airGO = new GameObject();
            airGO.SetActive(false);
            airGO.layer = 17;
            airGO.transform.parent = body.transform;

            SphereCollider SC = airGO.AddComponent<SphereCollider>();
            SC.isTrigger = true;
            SC.radius = airScale;

            SimpleFluidVolume SFV = airGO.AddComponent<SimpleFluidVolume>();
            SFV.SetValue("_layer", 5);
            SFV.SetValue("_priority", 1);
            SFV.SetValue("_density", 1.2f);
            SFV.SetValue("_fluidType", FluidVolume.Type.AIR);
            SFV.SetValue("_allowShipAutoroll", true);
            SFV.SetValue("_disableOnStart", false);

            if (isRaining)
            {
                VisorRainEffectVolume VREF = airGO.AddComponent<VisorRainEffectVolume>();
                VREF.SetValue("_rainDirection", VisorRainEffectVolume.RainDirection.Radial);
                VREF.SetValue("_layer", 0);
                VREF.SetValue("_priority", 0);

                AudioSource AS = airGO.AddComponent<AudioSource>();
                AS.mute = false;
                AS.bypassEffects = false;
                AS.bypassListenerEffects = false;
                AS.bypassReverbZones = false;
                AS.playOnAwake = false;
                AS.loop = true;
                AS.priority = 128;
                AS.volume = 0.35f;
                AS.pitch = 1f;
                AS.panStereo = 0f;
                AS.spatialBlend = 0f;
                AS.reverbZoneMix = 1f;

                OWAudioSource OWAS = airGO.AddComponent<OWAudioSource>();
                OWAS.SetAudioLibraryClip(AudioType.GD_RainAmbient_LP);
                OWAS.SetClipSelectionType(OWAudioSource.ClipSelectionOnPlay.RANDOM);
                OWAS.SetTrack(OWAudioMixer.TrackName.Environment);

                /*AudioVolume av = */
                airGO.AddComponent<AudioVolume>();
            }

            airGO.SetActive(true);
            Logger.Log("Finished building air.", Logger.LogType.Log);
        }
    }
}
