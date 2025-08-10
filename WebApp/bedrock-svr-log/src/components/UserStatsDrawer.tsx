import { Drawer, Box, Typography, IconButton, Avatar } from "@mui/material";
import { Close as CloseIcon } from "@mui/icons-material";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { useGetDurations } from "../Hooks/useGetDurations";
import { formatDateTime, formatTime } from "../Helpers/timeHelper";
import { useEffect } from "react";
import { useQueryClient } from "@tanstack/react-query";

const UserStatsDrawer = ({
  open,
  onClose,
}: {
  open: boolean;
  onClose: () => void;
}) => {
  const queryClient = useQueryClient();
  const { data: durations, isLoading } = useGetDurations();

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
      <Box className="p-6 h-full overflow-y-auto">
        <Box className="flex justify-between items-center mb-6">
          <Typography variant="h4" className="text-white font-bold">
            User Statistics
          </Typography>
          <IconButton
            onClick={onClose}
            className="text-gray-400 hover:text-white"
          >
            <CloseIcon />
          </IconButton>
        </Box>

        {/* User Stats Table */}
        <Box className="mb-8">
          <Typography variant="h6" className="text-white mb-4">
            Player Overview
          </Typography>
          <Box className="bg-gray-800 rounded-lg p-4">
            <DataTable
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
                body={(item) => <Avatar src={item.diceBearAvatarUrl} />}
              />
              <Column field="name" header="Username" className="text-white" />
              <Column
                field="isOnline"
                header="Online"
                className="text-white"
                body={(item) => (
                  <Typography>{item.isOnline ? "Yes" : "No"}</Typography>
                )}
              />

              <Column
                field="totalLiveDuration"
                header="Playtime"
                className="text-white"
                body={(item) => (
                  <Typography>{formatTime(item.totalLiveDuration)}</Typography>
                )}
              />
              <Column
                field="lastLogin"
                header="Last Seen"
                className="text-white"
                body={(item) => (
                  <Typography>{formatDateTime(item.lastLogin)}</Typography>
                )}
              />
            </DataTable>
          </Box>
        </Box>
      </Box>
    </Drawer>
  );
};

export default UserStatsDrawer;
