using SKitLs.Bots.Telegram.BotProcesses.Model;
using SKitLs.Bots.Telegram.DataBases.Model;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.resources.settings
{
    public class SkDBSettings
    {
        public static bool DisableRootPageMerging { get; set; } = false;

        public static string SourceSetId { get; set; } = "system.sourceSet";
        public static string DisplayPrefix { get; set; } = "display.db.";

        public static string RootPage_Id { get; set; } = "db.mng.temp";
        private static string _rootPage_LabelLK = "OpenDtMngMes";
        public static string RootPage_LabelLK
        {
            get => DisplayPrefix + _rootPage_LabelLK;
            set => _rootPage_LabelLK = value;
        }
        
        public static string DataTempPageId { get; set; } = "db.ds.temp";

        public static string OpenDatabaseCallbackId { get; set; } = "dt.mng";
        private static string _openDatabaseLK = "OpenDbMes";
        public static string OpenDatabaseLK
        {
            get => DisplayPrefix + _openDatabaseLK;
            set => _openDatabaseLK = value;
        }
        public static string OpenObjectCallbackId { get; set; } = "dt.obj";
        private static string _openObjectLK = "OpenObjMes";
        public static string OpenObjectLK
        {
            get => DisplayPrefix + _openObjectLK;
            set => _openObjectLK = value;
        }

        public static string AddNewCallbackId { get; set; } = "dt.add";
        private static string _addNewLK = "AddObjectMes";
        public static string AddNewLK
        {
            get => DisplayPrefix + _addNewLK;
            set => _addNewLK = value;
        }
        public static string EditExistingCallbackId { get; set; } = "dt.edt";
        private static string _editExistingLK = "EditObjectMes";
        public static string EditExistingLK
        {
            get => DisplayPrefix + _editExistingLK;
            set => _editExistingLK = value;
        }
        public static string RemoveExistingCallbackId { get; set; } = "dt.rmv";
        private static string _removeExistingLK = "RemoveObjectMes";
        public static string RemoveExistingLK
        {
            get => DisplayPrefix + _removeExistingLK;
            set => _removeExistingLK = value;
        }

        private static string _addStartupMessageLK = "AddStartupMessage";
        public static string AddStartupMessageLK
        {
            get => DisplayPrefix + _addStartupMessageLK;
            set => _addStartupMessageLK = value;
        }
        private static string _editStartupMessageLK = "EditStartupMessage";
        public static string EditStartupMessageLK
        {
            get => DisplayPrefix + _editStartupMessageLK;
            set => _editStartupMessageLK = value;
        }
        private static string _addItConfirmMessageLK = "AddItConfirmMessage";
        public static string AddItConfirmMessageLK
        {
            get => DisplayPrefix + _addItConfirmMessageLK;
            set => _addItConfirmMessageLK = value;
        }
        private static string _removeConfirmMessageLK = "RemoveConfirmMessage";
        public static string RemoveConfirmMessageLK
        {
            get => DisplayPrefix + _removeConfirmMessageLK;
            set => _removeConfirmMessageLK = value;
        }

        public static string TerminationalKeyLK { get; set; } = "system.db.TerminationalKey";
        public static string DefaultTerminationalKey { get; set; } = "Cancel";
        public static int DbProcessesInitState { get; set; } = 100;
        public static string AddProcessIdBase { get; set; } = "db.addObj";
        public static string EditProcessIdBase { get; set; } = "db.EditObj";
        public static string RemoveProcessIdBase { get; set; } = "db.RemoveObj";
        public static IST GetProcessData(IBotDataSet requester, DbActionType actionType, string? terminationalKey = null)
            => new(GetActionTypeId(actionType), GetActionTypeState(requester, actionType), terminationalKey ?? DefaultTerminationalKey);
        private static string GetActionTypeId(DbActionType actionType) => actionType switch
        {
            DbActionType.Add => AddProcessIdBase,
            DbActionType.Edit => EditProcessIdBase,
            DbActionType.Remove => RemoveProcessIdBase,
            _ => throw new NotImplementedException()
        };
        private static IUserState GetActionTypeState(IBotDataSet requester, DbActionType actionType)
            => new DefaultUserState(DbProcessesInitState + (int)requester.BotArgId * (int)actionType, $"state.{GetActionTypeId(actionType)}");
    }
}