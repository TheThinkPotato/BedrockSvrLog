import { Avatar, Box, Typography } from "@mui/material";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { formatDateTime } from "../../Helpers/timeHelper";
import { splitCamelCase } from "../../Helpers/textHelper";
import type { UserRealmEvent } from "../../Hooks/useGetUserRealmEvents";
import type { RealmEvent } from "../../Hooks/useGetRealmEvents";
import UserModal from "../Modals/UserModal/UserModal";

interface RealmTableProps {
  data: RealmEvent[] | UserRealmEvent[];
  isLoading: boolean;
  showAvatar?: boolean;
  showUsername?: boolean;
  showHeader?: boolean;
  RealmModal?: RealmModalProps;
  isInUserModal?: boolean;
}

interface RealmModalProps {
  open: boolean;
  onClose: () => void;
  setSelectedUser: (user: number) => void;
  setModalOpen: (open: boolean) => void;
  selectedUserXuid: number;
}

const RealmTable = ({
  data,
  isLoading,
  showAvatar = true,
  showUsername = true,
  showHeader = true,
  isInUserModal = false,
  RealmModal,
}: RealmTableProps) => {
  const hasModal = !!RealmModal?.open && !!RealmModal?.selectedUserXuid;

  return (
    <>
      <Box>
        {showHeader && (
          <Typography
            variant="h6"
            sx={{ marginLeft: "0.5rem", marginBottom: "0.5rem" }}
          >
            Recent Achievements
          </Typography>
        )}
        <Box className="bg-gray-800 rounded-lg p-4">
          <DataTable
            value={data ?? []}
            className="text-white"
            stripedRows
            size="small"
            loading={isLoading}
            {...(isInUserModal && { scrollable: true, scrollHeight: "170px" })}
          >
            {showAvatar && (
              <Column
                field="diceBearAvatarUrl"
                header=""
                className="text-white"
                body={(item) => <Avatar src={item.diceBearAvatarUrl} 
                sx={{
                  cursor: "pointer",
                  "&:hover": {
                    boxShadow: "0 0 5px 0 rgba(96, 165, 250, 0.8)",
                    transform: "scale(1.05)",
                  },
                }}
                onClick={() =>
                  RealmModal?.setSelectedUser(item.xuid)
                }
                />}
              />
            )}
            {showUsername && (
              <Column
                field="name"
                header="Username"
                className="text-white"
                body={(item) => (
                  <Box
                    onClick={() =>
                      RealmModal?.setSelectedUser(item.xuid)
                    }
                    style={{
                      cursor: "pointer",
                      textDecoration: "none",
                      display: "flex",
                      flexDirection: "row",
                      alignItems: "left",
                      justifyContent: "left",
                    }}
                    sx={{
                      "&:hover": { color: "#7cf" },
                    }}
                    className="hover:text-blue-400 transition-colors"
                  >
                    {item.name}
                  </Box>
                )}
              />
            )}
            <Column
              field="realmEvent"
              header="Event"
              className="text-white"
              {...(isInUserModal && { headerStyle: { width: "220px" } })}
              body={(item) => (
                <Typography>{splitCamelCase(item.realmEvent)}</Typography>
              )}
            />
            <Column
              field="eventTime"
              header="Time"
              className="text-white"
              body={(item) => (
                <Typography>{formatDateTime(item.eventTime)}</Typography>
              )}
            />
          </DataTable>
        </Box>
      </Box>
      {hasModal && (
      <UserModal
        selectedUserXuid={RealmModal?.selectedUserXuid ?? NaN}
          handleModalClose={RealmModal?.onClose}
          modalOpen={RealmModal?.open}
        />
      )}
    </>
  );
};

export default RealmTable;
