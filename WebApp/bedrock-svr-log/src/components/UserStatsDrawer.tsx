import { Drawer, Box, Typography, IconButton, Avatar } from "@mui/material";
import { CheckCircle, Close, Close as CloseIcon } from "@mui/icons-material";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { useGetDurations } from "../Hooks/useGetDurations";
import { formatDateTime, formatTimeCount } from "../Helpers/timeHelper";
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
      <Box className="p-6 h-full overflow-y-auto" sx={{ marginTop: "0.5rem" }}>
        <Box className="flex justify-between items-center" sx={{ marginLeft: "0.5rem" }}>           
          <Typography variant="h4">
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
          <Typography variant="h6"  sx={{ marginLeft: "0.5rem", marginBottom: "0.5rem" }}>
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
                className="text-white text-center"
                body={(item) => (
                    <Typography className="flex items-center">
                      {item.isOnline ? (
                        <CheckCircle
                          style={{ fontSize: "1.5rem", color: "#22c55e" }}
                        />                        
                      ) : (
                        <Close
                          style={{ fontSize: "1.5rem", color: "#ef4444" }}
                        />
                      )}
                    </Typography>
                )}
              />

              <Column
                field="totalLiveDuration"
                header="Playtime"
                className="text-white"
                body={(item) => (
                  <Typography>{formatTimeCount(item.totalLiveDuration)}</Typography>
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
