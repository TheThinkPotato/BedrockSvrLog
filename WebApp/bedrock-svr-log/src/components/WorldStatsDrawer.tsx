import { Drawer, Box, Typography, IconButton, Avatar } from "@mui/material";
import { Close as CloseIcon } from "@mui/icons-material";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { useGetRealmEvents } from "../Hooks/useGetRealmEvents";
import { formatDateTimeFull } from "../Helpers/timeHelper";
import { splitCamelCase } from "../Helpers/textHelper";

const WorldStatsDrawer = ({
  open,
  onClose,
}: {
  open: boolean;
  onClose: () => void;
}) => {
  const { data: realmEvents, isLoading: realmEventsLoading } =
    useGetRealmEvents();  

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
            World Achievements
          </Typography>
          <IconButton
            onClick={onClose}
            className="text-gray-400 hover:text-white"
          >
            <CloseIcon />
          </IconButton>
        </Box>

        {/* World Events Table */}
        <Box>
          <Typography variant="h6" className="text-white mb-4">
            Recent Events
          </Typography>
          <Box className="bg-gray-800 rounded-lg p-4">
            <DataTable
              value={realmEvents?.users ?? []}
              className="text-white"
              stripedRows
              size="small"
              loading={realmEventsLoading}
            >
              <Column
                field="diceBearAvatarUrl"
                header=""
                className="text-white"
                body={(item) => <Avatar src={item.diceBearAvatarUrl} />}
              />
              <Column field="name" header="Username" className="text-white" />
              <Column
                field="realmEvent"
                header="Event"
                className="text-white"
                body={(item) => <Typography>{splitCamelCase(item.realmEvent)}</Typography>}
              />
              <Column
                field="eventTime"
                header="Time"
                className="text-white"
                body={(item) => (
                  <Typography>{formatDateTimeFull(item.eventTime)}</Typography>
                )}
              />
            </DataTable>
          </Box>
        </Box>
      </Box>
    </Drawer>
  );
};

export default WorldStatsDrawer;
