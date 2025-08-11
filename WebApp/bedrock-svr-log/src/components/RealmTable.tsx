import { Avatar, Box, Typography } from "@mui/material";
import { Column } from "primereact/column";
import { DataTable } from "primereact/datatable";
import { formatDateTime } from "../Helpers/timeHelper";
import { splitCamelCase } from "../Helpers/textHelper";
import type { UserRealmEvent } from "../Hooks/useGetUserRealmEvetns";
import type { RealmEvent } from "../Hooks/useGetRealmEvents";

interface RealmTableProps {
    data: RealmEvent[] | UserRealmEvent[];
    isLoading: boolean;
    showAvatar?: boolean;
    showUsername?: boolean;
    showHeader?: boolean;
}

const RealmTable = ({ data, isLoading, showAvatar = true, showUsername = true, showHeader = true }: RealmTableProps) => {
    return(
        <Box>
            {showHeader && (
                <Typography variant="h6" sx={{ marginLeft: "0.5rem", marginBottom: "0.5rem" }}>
                    Recent Events
                </Typography>
            )}
          <Box className="bg-gray-800 rounded-lg p-4">
            <DataTable
              value={data ?? []}
              className="text-white"
              stripedRows
              size="small"
              loading={isLoading}
            >
                {showAvatar && (
              <Column
                field="diceBearAvatarUrl"
                header=""
                className="text-white"
                body={(item) => <Avatar src={item.diceBearAvatarUrl} />}
              />)}
              {showUsername && (
                <Column field="name" header="Username" className="text-white" />
              )}
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
                  <Typography>{formatDateTime(item.eventTime)}</Typography>
                )}
              />
            </DataTable>
          </Box>
        </Box>
    )
};

export default RealmTable;