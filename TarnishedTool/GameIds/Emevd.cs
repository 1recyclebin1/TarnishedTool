// 

using System.IO;

namespace TarnishedTool.GameIds;

public static class Emevd
{
    public class EmevdCommand(int groupId, int commandId, params object[] args)
    {
        public int GroupId { get; } = groupId;
        public int CommandId { get; } = commandId;
        public byte[] ParamData { get; } = Pack(args);

        private static byte[] Pack(object[] args)
        {
            if (args.Length == 0) return [];

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            int offset = 0;

            foreach (var arg in args)
            {
                int alignment = arg is sbyte or byte ? 1 : arg is short or ushort ? 2 : 4;
                int padding = (alignment - (offset % alignment)) % alignment;
                offset += padding + alignment;

                for (int i = 0; i < padding; i++) bw.Write((byte)0);

                switch (arg)
                {
                    case sbyte v: bw.Write(v); break;
                    case byte v: bw.Write(v); break;
                    case short v: bw.Write(v); break;
                    case ushort v: bw.Write(v); break;
                    case int v: bw.Write(v); break;
                    case uint v: bw.Write(v); break;
                    case float v: bw.Write(v); break;
                }
            }

            return ms.ToArray();
        }
    }

    public static class EmevdCommands
    {
        public static readonly EmevdCommand Rest = new(2004, 47);
        public static readonly EmevdCommand ReloadArea = new(2003, 70, (byte)1);

        public static readonly EmevdCommand SetMorning = new(
            2001,
            4,
            (byte)6, (byte)0, (byte)0, (byte)0, (byte)0, (byte)1, (float)0.75, (float)2.0, (float)0
        );

        public static readonly EmevdCommand SetNoon = new(
            2001,
            4,
            (byte)12, (byte)0, (byte)0, (byte)0, (byte)0, (byte)1, (float)0.75, (float)2.0, (float)0
        );

        public static readonly EmevdCommand SetDusk = new(
            2001,
            4,
            (byte)18, (byte)0, (byte)0, (byte)0, (byte)0, (byte)1, (float)0.75, (float)2.0, (float)0
        );

        public static readonly EmevdCommand SetNight = new(
            2001,
            4,
            (byte)20, (byte)0, (byte)0, (byte)0, (byte)0, (byte)1, (float)0.75, (float)2.0, (float)0
        );

        public static EmevdCommand SetWeather(sbyte type) => new(
            2003,
            68,
            type,
            (float)-1, (byte)1
        );

        public static EmevdCommand ForcePlaybackAnimation(uint entityId, int animationId) => new(
            2003,
            18,
            entityId,
            animationId,
            (byte)0, // shouldLoop
            (byte)0, // shouldWaitForCompletion
            (byte)0 // ignoreWaitForTransition
        );

        public static EmevdCommand ResetCharacterPosition(uint entityId) => new(2004, 81, entityId);

        public static EmevdCommand AwardItemsIncludingClients(int itemId) => new(2003, 36, itemId);

        public static EmevdCommand WaitForEventFlag(byte desiredFlagState, byte targetEventFlagType,
            uint targetEventFlagId) =>
            new(1003, 0, desiredFlagState, targetEventFlagType, targetEventFlagId);

        public static EmevdCommand PlayCutsceneToPlayerAndWarp(int cutsceneId, uint playbackMethod, uint areaEntityId,
            int mapId, uint playerEntityId, int unknown14, bool unknown18) => new(2002, 11, cutsceneId, playbackMethod,
            areaEntityId, mapId, playerEntityId, unknown14, unknown18);

        public static EmevdCommand PlayCutsceneToPlayer(int cutsceneId, uint playbackMethod, uint playerEntityId) =>
            new(2002, 3, cutsceneId, playbackMethod, playerEntityId);

        public static EmevdCommand WaitFixedTimeRealFrames(int numberOfFrames) => new(1001, 6, numberOfFrames);

        public static EmevdCommand SetEventFlag(byte targetEventFlagType, uint targetEventFlagId,
            byte desiredFlagState) => new(2003, 66, targetEventFlagType, targetEventFlagId, desiredFlagState);

        public static EmevdCommand ChangeCamera(int normalCameraId, int lockedCameraId) =>
            new(2008, 1, normalCameraId, lockedCameraId);

        public static EmevdCommand IssueShortWarpRequest(uint entityId, byte warpEntityType,
            uint warpDestinationEntityId, int dummypolyId) => new(2004, 41, entityId, warpEntityType,
            warpDestinationEntityId, dummypolyId);

        public static EmevdCommand SetCameraAngle(float xAngle, float yAngle) => new(2008, 04, xAngle, yAngle);

        public static EmevdCommand DisplayBossHealthBar(sbyte disabledEnabled, uint entityId, short slotNumber,
            int nameId) => new(2003, 11, disabledEnabled, entityId, slotNumber, nameId);

        public static EmevdCommand SetSpEffect(uint entityId, int spEffectId) => new(2004, 08, entityId, spEffectId);
        public static EmevdCommand ClearSpEffect(uint entityId, int spEffectId) => new(2004, 21, entityId, spEffectId);

        public static EmevdCommand ForceCharacterDeath(uint entityId, bool shouldReceiveRunes) =>
            new(2004, 4, entityId, shouldReceiveRunes);

        public static EmevdCommand CreateNpcPart(uint entityId, short npcPartId, short npcPartGroupIdx, int npcPartHp,
            float damageCorrection, float bodyDamageCompensation, bool isInvincible, bool startInStoppedState) =>
            new(2004, 22, entityId, npcPartId, npcPartGroupIdx, npcPartHp, damageCorrection, bodyDamageCompensation,
                isInvincible, startInStoppedState);

        public static EmevdCommand SetNpcPartSeAndSfx(uint entityId, int npcPartId, int defenseMaterialSeId,
            int defenseMaterialSfxId, int unknownA, int unknownB, int unknownC) =>
            new(2004, 24, entityId, npcPartId, defenseMaterialSeId, defenseMaterialSfxId, unknownA, unknownB, unknownC);

        public static EmevdCommand SetCharacterHpBarDisplay(uint entityId, byte disabledEnabled) =>
            new(2004, 30, entityId, disabledEnabled);

        public static EmevdCommand ForceAnimationPlayback(uint entityId, int animationId, bool shouldLoop,
            bool shouldWaitForCompletion, bool ignoreWaitForTransition, sbyte comparisonType, float numberOfTargets) =>
            new(2003, 18, entityId, animationId, shouldLoop, shouldWaitForCompletion, ignoreWaitForTransition,
                comparisonType, numberOfTargets);

        public static EmevdCommand WaitFixedTimeFrames(int timeFrames) => new(1001, 1, timeFrames);
        public static EmevdCommand RequestCharacterAiReplan(uint entityId, int aiId) => new(2004, 19, entityId, aiId);
        public static EmevdCommand WaitFixedTimeSeconds(float timeSeconds) => new(1001, 0, timeSeconds);

        public static EmevdCommand SetCharacterImmortality(uint entityId, byte disabledEnabled) =>
            new(2004, 12, entityId, disabledEnabled);

        public static EmevdCommand ShootBullet(uint bulletTeamEntityId, uint bulletProducerEntityId, int dummypolyId,
            int behaviorId, int firingAngleX, int firingAngleY, int firingAngleZ)
            => new(2003, 5, bulletTeamEntityId, bulletProducerEntityId, dummypolyId, behaviorId, firingAngleX,
                firingAngleY, firingAngleZ);
    }
}