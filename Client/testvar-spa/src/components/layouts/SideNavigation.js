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
                <ListItem key='usersets' disablePadding component={Link} to={{pathname: `/users/${auth.userId}/sets`}} style={{ color: '#FFF' }}>
                    <ListItemButton secondary>
                        <ListItemIcon>
                            <QuizIcon />
                        </ListItemIcon>
                        <ListItemText primary='My Flashcard Sets' />
                    </ListItemButton>
                </ListItem>
                <ListItem key='collections' disablePadding>
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
                    <Divider />
                    <List>
                        <ListItem key='user' disablePadding>
                            <ListItemButton>
                                <ListItemIcon secondary>
                                    <PeopleIcon />
                                </ListItemIcon>
                                <ListItemText secondary='Users' />
                            </ListItemButton>
                        </ListItem>
                        <ListItem key='settings' disablePadding>
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