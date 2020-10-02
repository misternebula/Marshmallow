![logo](logo.png)

A planet creator for Outer Wilds.

Planets are created using a JSON file format structure, and placed in the `planets` folder.

There is an example planet bundled with the mod - change some values and see what happens!

<!-- TOC -->

- [Creating a planet file](#creating-a-planet-file-)
- [Using Marshmallow from other mods](#using-marshmallow-from-other-mods-)
- [Submit bugs / Chat about life](#submit-bugs--chat-about-life-)
- [Credits](#credits-)

<!-- /TOC -->

## Creating a planet file :
This list will update as more options are added. Here is an example of a file :
```
{
	"Name" : "Gallifrey",
	"Position" : {
		"x" : 0,
		"y" : 0,
		"z" : 10000
	},
	"OrbitAngle" : 45,
	"PrimaryBody" : "SUN",
	"IsMoon" : false,
	"HasSpawnPoint" : true,
	"HasClouds" : true,
	"TopCloudSize" : 650,
	"BottomCloudSize" : 600,
	"AtmoEndSize" : 650,
	"BottomCloudTint" : 
	{
		"r" : 255,
		"g" : 0,
		"b" : 0,
		"a" : 128
	},
	"TopCloudTint" : 
	{
		"r" : 255,
		"g" : 0,
		"b" : 0,
		"a" : 128
	},
	"HasWater" : true,
	"WaterSize" : 401,
	"HasRain" : true,
	"HasGravity" : true,
	"SurfaceAcceleration" : 12,
	"HasMapMarker" : true,
	"HasFog" : true,
	"FogTint" : 
	{
		"r" : 255,
		"g" : 100,
		"b" : 0,
		"a" : 128
	},
	"FogDensity" : 0.75,
	"GroundSize" : 400,
	"HasGround": true,
	"LightTint" : 
	"IsTidallyLocked" : false,
	{
		"r" : 255,
		"g" : 0,
		"b" : 255,
		"a" : 128
	}
}
```
Everything in "Required" is always needed, and so is every tabbed line in an option.
### Required :
- Name - The name of the planet.
- Position - The Vector3 positon of the planet in world space.
- OrbitAngle - The angle of the orbit.
- PrimaryBody - The AstroObject ID of the body this planet orbits.
- IsMoon - Is the body a moon or not a moon (a planet)?

### Optional :
- HasGround - Set to "true" if you want to have a sphere as a ground.
  - GroundSize - The size of the ground sphere.
- HasClouds - Set to "true" if you want Giant's Deep-type clouds.
  - TopCloudSize - The size of the outer sphere of the clouds.
  - BottomCloudSize - The size of the bumpy clouds underneath the top. *(Check that the bottom clouds are not poking through the top!)*
  - TopCloudTint - The color of the top clouds.
  - BottomCloudTint - The color of the bottom clouds.
- HasWater - Set to "true" if you want water.
  - WaterSize - Size of the water sphere.
- HasRain - Set to "true" if you want it to be raining.
- HasGravity - Set to "true" if you want gravity.
  - SurfaceAcceleration - Strength of gravity.
- HasMapMarker - Set to "true" if you want the planet name on the map.
- HasFog - Set to "true" if you want fog.
  - FogTint - The color of the fog.
  - FogDensity - The thickness of the fog. \[0-1]
  
## Using Marshmallow from other mods :
Marshmallow uses the fancy API system provided by OWML to allow other mods to communicate with it. As this system is currently still being worked on (by me :P), the way the Marshmallow API is used is slightly more annoying than when the system will be finished.

To use the API, first define this interface in your mod.
```
public interface IMarshmallow
{
    void Create(Dictionary<string, object> config);
    
    GameObject GetPlanet(string name);
}
```
Then, you need to find the Marshmallow API. This can be done using the interaction helpers of OWML.
```
var marshmallowApi = ModHelper.Interaction.GetApi<IMarshmallow>("misternebula.Marshmallow");
```
**Make sure that Marshmallow is defined as a dependency! This will prevent any load-order issues from occuring.**
Next, we need to generate the dictionary of config options. To save time, you can copy this definition and just change the values.
```
var configDict = new Dictionary<string, object>
                {
                    { "Name", "Test Planet" },
                    { "Position", new Vector3(0, 0, 3000) },
                    { "OrbitAngle", 0 },
                    { "PrimaryBody", "SUN" },
		    { "IsMoon" : false },
		    { "AtmoEndSize", 0f },
                    { "HasSpawnPoint", true },
                    { "HasGround", true },
                    { "GroundSize", 400f },
                    { "HasClouds", true },
                    { "TopCloudSize", 650f },
                    { "BottomCloudSize", 600f },
                    { "TopCloudTint", new Color32(255, 0, 0, 128) },
                    { "BottomCloudTint", new Color32(255, 0, 0, 128) },
                    { "HasWater", true },
                    { "WaterSize", 401f },
                    { "HasRain", true },
                    { "HasGravity", true },
                    { "SurfaceAcceleration", 12f },
                    { "HasMapMarker", true },
                    { "HasFog", true },
                    { "FogTint", new Color32(255, 0, 0, 128) },
                    { "FogDensity", 0.5f },
		    { "HasGround", true },
                    { "GroundSize", 150f },
                    { "IsTidallyLocked", false},
                    { "LightTint", new Color32(255, 0, 0, 128) },
                };
```
Then, you just call the `Create` method in the API!
```
marshmallowApi.Create(configDict);
```
## Submit bugs / Chat about life :
Did you know we have a nice [Outer Wilds discord](https://discord.gg/Sftcc9Z)? Drop by to submit bugs, ask questions, and chat about whatever.
Join us modding folk in the `#modding` channel!

## Credits :
Written by Mister_Nebula

With help from :
- TAImatem
- AmazingAlek
- Raicuparta
- and the Outer Wilds discord server.
