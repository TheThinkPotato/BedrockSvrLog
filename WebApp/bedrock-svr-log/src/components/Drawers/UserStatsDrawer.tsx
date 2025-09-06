import { Drawer, Box, Typography, IconButton, Avatar } from "@mui/material";
import { CheckCircle, Close, Close as CloseIcon } from "@mui/icons-material";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { useGetDurations, type Duration } from "../../Hooks/useGetDurations";
import {
  formatDateTime,
  formatTimeCount,
  getTimeDifferenceTimeDateFull,
} from "../../Helpers/timeHelper";
import { useEffect, useState } from "react";
import { useQueryClient } from "@tanstack/react-query";
import UserModal from "../Modals/UserModal/UserModal";

const UserStatsDrawer = ({
  open,
  onClose,
}: {
  open: boolean;
  onClose: () => void;
}) => {
  const queryClient = useQueryClient();
  const { data: durations, isLoading } = useGetDurations(open);

  const [currentDateTime, setCurrentDateTime] = useState(new Date());
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedUser, setSelectedUser] = useState<number>(NaN);

  const isAnyoneOnline = durations?.durations.some((item) => item.isOnline);

  const handleUserClick = (user: Duration) => {
    setSelectedUser(user.xuid);
    setModalOpen(true);
  };

  const handleModalClose = () => {
    setModalOpen(false);
    setSelectedUser(NaN);
  };

  useEffect(() => {
    if (isAnyoneOnline) {
      const interval = setInterval(() => {
        setCurrentDateTime(new Date()); // update to current time
      }, 1000);

      return () => clearInterval(interval); // cleanup on unmount
    }
  }, [isAnyoneOnline]);

  useEffect(() => {
    if (open) {
      queryClient.invalidateQueries({ queryKey: ["durations"] });
    }
  }, [open, queryClient]);

  return (
    <Drawer
      anchor="right"
      open={open}
      onClose={onClose}
      PaperProps={{
        sx: {
          width: 800,
          backgroundColor: "#1f2937",
          color: "white",
        },
      }}
    >
      <Box className="p-6 h-full overflow-y-auto" sx={{ marginTop: "0.5rem" }}>
        <Box
          className="flex justify-between items-center"
          sx={{ marginLeft: "0.5rem" }}
        >
          <Typography variant="h4">User Statistics</Typography>
          <IconButton
            onClick={onClose}
            className="text-gray-400 hover:text-white"
          >
            <CloseIcon />
          </IconButton>
        </Box>

        {/* User Stats Table */}
        <Box className="mb-8">
          <Typography
            variant="h6"
            sx={{ marginLeft: "0.5rem", marginBottom: "0.5rem" }}
          >
            Player Overview
          </Typography>
          <Box className="bg-gray-800 rounded-lg p-4">
            <DataTable
              key={isAnyoneOnline ? currentDateTime.getTime() : null}
              value={durations?.durations ?? []}
              className="text-white"
              stripedRows
              size="small"
              loading={isLoading}
            >
              <Column
                field="diceBearAvatarUrl"
                header=""
                className="text-white"
                body={(item) => (
                  <Avatar
                    src={item.diceBearAvatarUrl}
                    sx={{
                      cursor: "pointer",
                      "&:hover": {
                        boxShadow: "0 0 5px 0 rgba(96, 165, 250, 0.8)",
                        transform: "scale(1.05)",
                      },
                    }}
                    onClick={() => handleUserClick(item)}
                  />
                )}
              />
              <Column
                field="name"
                header="Username"
                className="text-white"
                body={(item) => (
                  <Box
                    onClick={() => handleUserClick(item)}
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
              <Column
                field="isOnline"
                header="Online"
                className="text-white text-center"
                body={(item: Duration) => (
                  <Typography className="flex items-center">
                    {item.isOnline ? (
                      <CheckCircle
                        style={{ fontSize: "1.5rem", color: "#22c55e" }}
                      />
                    ) : (
                      <Close style={{ fontSize: "1.5rem", color: "#ef4444" }} />
                    )}
                  </Typography>
                )}
              />

              <Column
                field="lastSpawnTime"
                header="Online Time"
                className="text-white"
                body={(item) => (
                  <Typography>{`${
                    item.isOnline
                      ? getTimeDifferenceTimeDateFull(
                          currentDateTime,
                          item.spawnTime
                        )
                      : "-"
                  }`}</Typography>
                )}
              />
              <Column
                field="totalLiveDuration"
                header="Total Playtime"
                className="text-white"
                body={(item) => (
                  <Typography>
                    {formatTimeCount(item.totalLiveDuration)}
                  </Typography>
                )}
              />
              <Column
                field="lastLogOut"
                header="Last Seen"
                className="text-white"
                body={(item) => (
                  <Typography>
                    {item.lastLogOut && item.lastLogOut !== ""
                      ? formatDateTime(item.lastLogOut)
                      : formatDateTime(item.lastLogin)}
                  </Typography>
                )}
              />
            </DataTable>
          </Box>
        </Box>
      </Box>

      <UserModal
        selectedUserXuid={selectedUser}
        handleModalClose={handleModalClose}
        modalOpen={modalOpen}
      />
    </Drawer>
  );
};

export default UserStatsDrawer;
