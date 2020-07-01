using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Oxide.Plugins
{
	[Info("Enderpearl", "Wolfleader101", "0.3.0")]
	[Description("Throw an ender pearl and teleport to its location")]
	class Enderpearl : RustPlugin
	{
		#region Variables
		
		private PluginConfig config;
		public const string enderPearlPerms = "enderpearl.use";

		#endregion

		#region Hooks
		private void Init()
		{
			config = Config.ReadObject<PluginConfig>();

			permission.RegisterPermission(enderPearlPerms, this);
		}

		void OnPlayerAttack(BasePlayer attacker, HitInfo info)
		{
			if (permission.UserHasPermission(attacker.UserIDString, enderPearlPerms))
			{
				if (info == null) return;
				if (info.IsProjectile())
				{
					string EntName = info.ProjectilePrefab.name;
					string projectileName = config.enderpearl + ".projectile";

					if (EntName == projectileName)
					{
						Teleport(attacker, info);
					}
					else if (EntName == config.enderpearl) // some things like ammo are riflebullet and dont contain a .projectile
					{
						Teleport(attacker, info);
					}


				}
			}

		}
		#endregion

		#region Custom Methods
		void Teleport(BasePlayer attacker, HitInfo info)
		{
			Vector3 EntLoc = info.HitPositionWorld;
			attacker.transform.position = EntLoc;
			info.ProjectilePrefab.conditionLoss += 1f;
		}

		#endregion

		#region Config

		private class PluginConfig
		{
			[JsonProperty("Enderpearl")]
			public string enderpearl { get; set; }


		}

		private PluginConfig GetDefaultConfig()
		{
			return new PluginConfig
			{
				enderpearl = "snowball"
			};
		}

		protected override void LoadDefaultConfig()
		{
			Config.WriteObject(GetDefaultConfig(), true);
		}
		#endregion
	}
}