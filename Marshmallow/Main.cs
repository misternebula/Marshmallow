using Marshmallow.Atmosphere;
using Marshmallow.Body;
using Marshmallow.External;
using Marshmallow.General;
using Marshmallow.Utility;
using OWML.Common;
using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Marshmallow.Utility.Logger;

namespace Marshmallow
{
    public class Main : ModBehaviour
    {
        public static IModHelper helper;

        public static List<MarshmallowBody> BodyList = new List<MarshmallowBody>();

        private bool finishNextUpdate = false;

        public override object GetApi()
        {
            return new MarshmallowApi();
        }

        void Start()
        { 
            SceneManager.sceneLoaded += OnSceneLoaded;
            helper = base.ModHelper;

            Logger.Log("Begin load of config files...", Logger.LogType.Log);

            try
            {
                foreach (var file in Directory.GetFiles(ModHelper.Manifest.ModFolderPath + @"planets\"))
                {
                    var config = ModHelper.Storage.Load<PlanetConfig>(file.Replace(ModHelper.Manifest.ModFolderPath, ""));
                    BodyList.Add(new MarshmallowBody(config));

                    Logger.Log("* " + config.Name + " at position " + config.Position.ToVector3() + " relative to " + config.PrimaryBody + ". Moon? : " + config.IsMoon, Logger.LogType.Log);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error! - " + ex.Message, Logger.LogType.Error);
            }

            if (BodyList.Count != 0)
            {
                Logger.Log("Loaded [" + BodyList.Count + "] config files.", Logger.LogType.Log);
            }
            else
            {
                Logger.Log("No config files found!", Logger.LogType.Warning);
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "SolarSystem")
            {
                return;
            }

            foreach (var body in BodyList)
            {
                var planetObject = GenerateBody(body);

                var primayBody = Locator.GetAstroObject(AstroObject.StringIDToAstroObjectName(body.Config.PrimaryBody));

                planetObject.transform.parent = Locator.GetRootTransform();
                planetObject.transform.position = primayBody.gameObject.transform.position + body.Config.Position.ToVector3();
                planetObject.SetActive(true);
            }

            finishNextUpdate = true;
        }

        void Update()
        {
            if (finishNextUpdate)
            {
                foreach (var body in BodyList)
                {
                    OrbitlineBuilder.Make(body.Object, body.Object.GetComponent<AstroObject>());
                }
                finishNextUpdate = false;
            }
        }

        public static GameObject GenerateBody(MarshmallowBody body)
        {
            Logger.Log("Begin generation sequence of [" + body.Config.Name + "] ...", Logger.LogType.Log);

            var go = new GameObject(body.Config.Name);
            go.SetActive(false);

            GeometryBuilder.Make(go, body.Config.GroundSize);

            var outputTuple = BaseBuilder.Make(go, Locator.GetAstroObject(AstroObject.StringIDToAstroObjectName(body.Config.PrimaryBody)), body.Config);

            var owRigidbody = (OWRigidbody)outputTuple.Items[1];
            RFVolumeBuilder.Make(go, owRigidbody, body.Config);

            if (body.Config.HasMapMarker)
            {
                MarkerBuilder.Make(go, body.Config);
            }

            var sector = MakeSector.Make(go, owRigidbody, body.Config);

            if (body.Config.HasClouds)
            {
                CloudsBuilder.Make(go, sector, body.Config);
                SunOverrideBuilder.Make(go, sector, body.Config);
            }

            AirBuilder.Make(go, body.Config.TopCloudSize / 2, body.Config.HasRain);

            if (body.Config.HasWater)
            {
                WaterBuilder.Make(go, sector, body.Config);
            }

            EffectsBuilder.Make(go, sector);
            VolumesBuilder.Make(go, body.Config);
            AmbientLightBuilder.Make(go, sector, body.Config);
            AtmosphereBuilder.Make(go, body.Config);

            Logger.Log("Generation of [" + body.Config.Name + "] completed.", Logger.LogType.Log);

            body.Object = go;

            return go;
        }

        public static void CreateBody(MarshmallowBody body)
        {
            var planet = GenerateBody(body);

            planet.transform.parent = Locator.GetRootTransform();
            planet.transform.position = Locator.GetAstroObject(AstroObject.StringIDToAstroObjectName(body.Config.PrimaryBody)).gameObject.transform.position + body.Config.Position.ToVector3();
            planet.SetActive(true);

            planet.GetComponent<OWRigidbody>().SetVelocity(Locator.GetCenterOfTheUniverse().GetOffsetVelocity());

            var primary = Locator.GetAstroObject(AstroObject.StringIDToAstroObjectName(body.Config.PrimaryBody)).GetAttachedOWRigidbody();
            var initialMotion = primary.GetComponent<InitialMotion>();
            if (initialMotion != null)
            {
                planet.GetComponent<OWRigidbody>().AddVelocityChange(-initialMotion.GetInitVelocity());
                planet.GetComponent<OWRigidbody>().AddVelocityChange(primary.GetVelocity());
            }
        }
    }

    public class MarshmallowApi
    {
        public void Create(Dictionary<string, object> config)
        {
            Logger.Log("Recieved API request to create planet " + (string)config["Name"] + " at position " + (Vector3)config["Position"], Logger.LogType.Log);
            var planetConfig = new PlanetConfig
            {
                Name = (string)config["Name"],
                Position = new MVector3(((Vector3)config["Position"]).x, ((Vector3)config["Position"]).y, ((Vector3)config["Position"]).z),
                OrbitAngle = (int)config["OrbitAngle"],
                IsMoon = (bool)config["IsMoon"],
                AtmoEndSize = (float)config["AtmoEndSize"],
                PrimaryBody = (string)config["PrimaryBody"],
                HasClouds = (bool)config["HasClouds"],
                TopCloudSize = (float)config["TopCloudSize"],
                BottomCloudSize = (float)config["BottomCloudSize"],
                TopCloudTint = new MColor32(((Color32)config["TopCloudTint"]).r, ((Color32)config["TopCloudTint"]).g, ((Color32)config["TopCloudTint"]).b, ((Color32)config["TopCloudTint"]).a),
                BottomCloudTint = new MColor32(((Color32)config["BottomCloudTint"]).r, ((Color32)config["BottomCloudTint"]).g, ((Color32)config["BottomCloudTint"]).b, ((Color32)config["BottomCloudTint"]).a),
                HasWater = (bool)config["HasWater"],
                WaterSize = (float)config["WaterSize"],
                HasRain = (bool)config["HasRain"],
                HasGravity = (bool)config["HasGravity"],
                SurfaceAcceleration = (float)config["SurfaceAcceleration"],
                HasMapMarker = (bool)config["HasMapMarker"],
                HasFog = (bool)config["HasFog"],
                FogTint = new MColor32(((Color32)config["FogTint"]).r, ((Color32)config["FogTint"]).g, ((Color32)config["FogTint"]).b, ((Color32)config["FogTint"]).a),
                FogDensity = (float)config["FogDensity"],
                HasGround = (bool)config["HasGround"],
                GroundSize = (float)config["GroundSize"],
                IsTidallyLocked = (bool)config["IsTidallyLocked"],
                LightTint = new MColor32(((Color32)config["LightTint"]).r, ((Color32)config["LightTint"]).g, ((Color32)config["LightTint"]).b, ((Color32)config["LightTint"]).a),
            };

            var body = new MarshmallowBody(planetConfig);

            Main.BodyList.Add(body);

            Main.helper.Events.Unity.RunWhen(() => Locator.GetCenterOfTheUniverse() != null, () => Main.CreateBody(body));
        }

        public GameObject GetPlanet(string name)
        {
            return Main.BodyList.FirstOrDefault(x => x.Config.Name == name).Object;
        }
    }
}
