import {format} from "date-fns";

export function formatTimestamp(dateString) {
    const date = Date.parse(dateString);
    return format(date, 'dd-MMM-yyyy, hh:mm:ss');
}