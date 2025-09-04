import { Drawer, Box, Typography, IconButton } from "@mui/material";
import { Close as CloseIcon } from "@mui/icons-material";
import { useGetRealmEvents } from "../../Hooks/useGetRealmEvents";
import RealmTable from "../Tables/RealmTable";
import { useState } from "react";

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
  const [selectedUserXuid, setSelectedUserXuid] = useState<number>(NaN);

  const handleUserClick = (userXuid: number) => {
    setSelectedUserXuid(userXuid);
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
            selectedUserXuid: selectedUserXuid,
          }}
        />
      </Box>
    </Drawer>
  );
};

export default WorldStatsDrawer;
