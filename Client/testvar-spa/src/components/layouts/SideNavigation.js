import {useAuth} from "../../hooks/AuthProvider";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import {Link} from "react-router-dom";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import HomeIcon from "@mui/icons-material/Home";
import ListItemText from "@mui/material/ListItemText";
import QuizIcon from "@mui/icons-material/Quiz";
import BookmarksIcon from "@mui/icons-material/Bookmarks";
import Divider from "@mui/material/Divider";
import PeopleIcon from "@mui/icons-material/People";
import SettingsIcon from "@mui/icons-material/Settings";
import * as React from "react";
import InventoryIcon from '@mui/icons-material/Inventory';

const SideNavigation = () => {
    const auth = useAuth();

    return (
        <>
            <List>
                <ListItem key='home' disablePadding component={Link} to={{pathname: `/sets`}} style={{ color: '#FFF' }}>
                    <ListItemButton secondary>
                        <ListItemIcon secondary>
                            <HomeIcon />
                        </ListItemIcon>
                        <ListItemText primary='Home' />
                    </ListItemButton>
                </ListItem>
                <ListItem key='collections' disablePadding component={Link} to={{pathname: `/collections`}} style={{ color: '#FFF' }}>
                    <ListItemButton secondary>
                        <ListItemIcon secondary>
                            <InventoryIcon />
                        </ListItemIcon>
                        <ListItemText primary='Collections' />
                    </ListItemButton>
                </ListItem>
                <Divider sx={{mt: 2, mb: 2}}/>
                <ListItem key='usersets' disablePadding component={Link} to={{pathname: `/users/${auth.userId}/sets`}} style={{ color: '#FFF' }}>
                    <ListItemButton secondary>
                        <ListItemIcon>
                            <QuizIcon />
                        </ListItemIcon>
                        <ListItemText primary='My Flashcard Sets' />
                    </ListItemButton>
                </ListItem>
                <ListItem key='usercollections' disablePadding component={Link} to={{pathname: `/users/${auth.userId}/collections`}} style={{ color: '#FFF' }}>
                    <ListItemButton>
                        <ListItemIcon>
                            <BookmarksIcon />
                        </ListItemIcon>
                        <ListItemText primary='My Collections' />
                    </ListItemButton>
                </ListItem>
            </List>
            {
                auth.isAdmin &&
                <>
                    <Divider sx={{mt: 2, mb: 2}}/>
                    <List>
                        <ListItem key='user' disablePadding component={Link} to={{pathname: `/users`}} style={{ color: '#FFF'}}>
                            <ListItemButton>
                                <ListItemIcon secondary>
                                    <PeopleIcon />
                                </ListItemIcon>
                                <ListItemText secondary='Users' />
                            </ListItemButton>
                        </ListItem>
                        <ListItem key='settings' disablePadding component={Link} to={{ pathname: `/settings`}} style={{ color: '#FFF'}}>
                            <ListItemButton>
                                <ListItemIcon secondary>
                                    <SettingsIcon />
                                </ListItemIcon>
                                <ListItemText secondary='Settings' />
                            </ListItemButton>
                        </ListItem>
                    </List>
                </>
            }
        </>
    );
}
export default SideNavigation;