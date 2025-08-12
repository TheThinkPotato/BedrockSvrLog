import { Drawer, Box, Typography, IconButton } from "@mui/material";
import { Close as CloseIcon } from "@mui/icons-material";
import { useGetRealmEvents } from "../Hooks/useGetRealmEvents";
import RealmTable from "./RealmTable";
import { useState } from "react";
import type { Duration } from "../Hooks/useGetDurations";

const WorldStatsDrawer = ({
  open,
  onClose,
}: {
  open: boolean;
  onClose: () => void;
}) => {
  const { data: realmEvents, isLoading: realmEventsLoading } =
    useGetRealmEvents(open);

  const [modalOpen, setModalOpen] = useState(false);
  const [selectedUser, setSelectedUser] = useState<Duration | null>(null);

  const handleUserClick = (user: Duration) => {
    setSelectedUser(user);
    setModalOpen(true);
  };

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
          <Typography variant="h4">World Achievements</Typography>
          <IconButton
            onClick={onClose}
            className="text-gray-400 hover:text-white"
          >
            <CloseIcon />
          </IconButton>
        </Box>

        <RealmTable
          data={realmEvents ?? []}
          isLoading={realmEventsLoading}
          RealmModal={{
            open: modalOpen,
            onClose: () => setModalOpen(false),
            setSelectedUser: handleUserClick,
            setModalOpen: (open: boolean) => setModalOpen(open),
            selectedUser: selectedUser,
          }}
        />
      </Box>
    </Drawer>
  );
};

export default WorldStatsDrawer;
