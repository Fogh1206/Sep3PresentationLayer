package PersistenceService;

import PersistenceServer.Group;
import PersistenceServer.Invitation;
import PersistenceServer.Note;
import PersistenceServer.User;
import com.google.gson.Gson;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class PersistenceService implements IPersistenceService {

    private static final Gson gson = new Gson();
    private final String URL = "jdbc:postgresql://tai.db.elephantsql.com:5432/seitjdhj";
    private final String USER = "seitjdhj";
    private final String PASSWORD = "9LEmAjua_Uo0YR5sGqAFHn0Kgm9DDKu1";
    private final Connection connection = DriverManager.getConnection(URL, USER, PASSWORD);


    public PersistenceService() throws SQLException {
    }

    @Override
    public String postGroup(String json) {
        String insertString =
                "INSERT INTO notelender.groups (groupname) VALUES (?)";
        List<Group> GroupList = new ArrayList<>();
        try {
            PreparedStatement insertGroup = connection.prepareStatement(insertString,PreparedStatement.RETURN_GENERATED_KEYS);
            insertGroup.setString(1, json);
            insertGroup.executeUpdate();


            try (ResultSet generatedKeys =  insertGroup.getGeneratedKeys()) {
                if (generatedKeys.next()) {
                    Group groupToAdd = new Group(generatedKeys.getInt(1), generatedKeys.getString(2));
                    GroupList.add(groupToAdd);
                    return gson.toJson(GroupList);
                } else {
                    throw new SQLException("Creating failed, no ID obtained.");
                }
            }
        } catch (SQLException e) {
            System.out.println("Connection failure.");
            e.printStackTrace();
        }
//            Statement statement = connection.createStatement();
//            statement.executeUpdate("INSERT INTO notelender.groups (name) VALUES (" + json + ")", Statement.RETURN_GENERATED_KEYS);
//            try (ResultSet generatedKeys = statement.getGeneratedKeys()) {
//                if (generatedKeys.next()) {
//                    Group groupToAdd = new Group(generatedKeys.getInt(1), generatedKeys.getString(2));
//                    GroupList.add(groupToAdd);
//                    return gson.toJson(GroupList);
//                } else {
//                    throw new SQLException("Creating failed, no ID obtained.");
//                }
//            }
//        } catch (SQLException e) {
//            System.out.println("Connection failure.");
//            e.printStackTrace();
//        }
        return null;
    }

    @Override
    public String addNote(String json) throws SQLException {
        System.out.println("JSON: " + json);
        Note note = new Note(0,0,0,0,0,"","","");
        note = gson.fromJson(json,Note.class);
        System.out.println("Note: " + note.getId());

        try {
            Statement statement = connection.createStatement();
            statement.executeUpdate("INSERT INTO notelender.notes (id,user_id,group_id,week,year,name,status,text) VALUES ("
                    + note.getId() + "," + note.getUserId() + ","
                    + note.getGroupId() + "," + note.getWeek() + ","
                    + note.getYear() + ",'" + note.getName() + "','"
                    + note.getStatus() + "','" + note.getText() + "')", Statement.RETURN_GENERATED_KEYS);
            try (ResultSet generatedKeys = statement.getGeneratedKeys()) {
                if (generatedKeys.next()) {
                    Note noteToAdd = new Note(generatedKeys.getInt(1), generatedKeys.getInt(2),
                            generatedKeys.getInt(3), generatedKeys.getInt(4),
                            generatedKeys.getInt(5), generatedKeys.getString(6),
                            generatedKeys.getString(7), generatedKeys.getString(8));
                    return gson.toJson(noteToAdd);
                } else {
                    throw new SQLException("Creating failed, no ID obtained.");
                }
            }
        } catch (SQLException e) {
            System.out.println("Connection failure.");
            e.printStackTrace();
        }
        return null;
    }

    @Override
    public String deleteGroup(int id) throws SQLException {
        PreparedStatement statement = connection.prepareStatement
                ("DELETE FROM notelender.groups WHERE id='" + id + "'");
        int deleted = statement.executeUpdate();
        if (deleted == 0) {
            return "Fail";
        } else {
            return "Success";
        }
    }

    @Override
    public String getNote(int id) throws SQLException {
        List<Note> NoteList = new ArrayList<>();
        ResultSet resultSet = connection.createStatement().executeQuery
                ("SELECT * FROM notelender.notes WHERE group_id = " + id);
        while (resultSet.next()) {
            Note noteToAdd = new Note(resultSet.getInt(1), resultSet.getInt(2),
                    resultSet.getInt(3), resultSet.getInt(4),
                    resultSet.getInt(5), resultSet.getString(6),
                    resultSet.getString(7), resultSet.getString(8));
            NoteList.add(noteToAdd);
        }
        return gson.toJson(NoteList);
    }

    @Override
    public String getGroup(int id) throws SQLException {
        List<Group> GroupList = new ArrayList<>();
        ResultSet resultSet = connection.createStatement().executeQuery
                ("SELECT * FROM notelender.groups WHERE id = " + id);
        while (resultSet.next()) {
            Group groupToAdd = new Group(resultSet.getInt(1), resultSet.getString(2));
            GroupList.add(groupToAdd);
        }
        return gson.toJson(GroupList);
    }

    @Override
    public String getUserList(int id) throws SQLException {
        String text = "";
        ResultSet resultSet = connection.createStatement().executeQuery
                ("SELECT * FROM sep3.notes WHERE id = " + id);
        while (resultSet.next()) {
            text = resultSet.getString(2);
            System.out.println(text);
        }
        List<String> AdultsList = new ArrayList<>();
        AdultsList.add(text);
        return gson.toJson(AdultsList);
    }

    /*
        Note section
     */

    @Override
    public String validateUser(String json) throws SQLException {

        User user = null;
        System.out.println("aleo");
        User temp=gson.fromJson(json,User.class);
        try {

            ResultSet resultSet = connection.createStatement().executeQuery
                    ("SELECT * FROM notelender.users WHERE username ='" + temp.getUsername() + "'");
            while (resultSet.next()) {
                user = new User(resultSet.getInt(1), resultSet.getString(2), resultSet.getString(3), resultSet.getString(4), resultSet.getString(5));
            }
            System.out.println("Login is working");
            return gson.toJson(user);
        } catch (Exception e) {
            return null;
        }

    }

    @Override
    public String registerUser(String json) throws SQLException {
        User temp=gson.fromJson(json,User.class);
        User user;
        try {
            Statement statement = connection.createStatement();
            statement.executeUpdate("INSERT INTO notelender.users (firstname,lastname,username,password) VALUES ('" + temp.getFirstName() + "','" + temp.getLastName() + "','" + temp.getUsername()+ "','" + temp.getPassword() + "')", Statement.RETURN_GENERATED_KEYS);
            try (ResultSet generatedKeys = statement.getGeneratedKeys()) {
                if (generatedKeys.next()) {
                    user = new User(generatedKeys.getInt(1), generatedKeys.getString(2), generatedKeys.getString(3), generatedKeys.getString(4), generatedKeys.getString(5));
                    System.out.println("register User is working");
                    return gson.toJson(user);
                } else {
                    throw new SQLException("Creating failed, no ID obtained.");
                }
            }
        } catch (SQLException e) {
            System.out.println("Connection failure.");
            e.printStackTrace();
        }
        return null;
    }

    @Override
    public String addInvitation(int id) throws SQLException {
        {
            /*
//            Invitation temp = null;
            Invitation invitation = null;
            Group group = null;

            String sqlQuery = "INSERT INTO notelender.invitation(id, invitor_id, invitee_id, group_id) VALUES (" +
                    invitation.getId() + "," + invitation.getInvitorId() + "," + invitation.getInviteeId() + "," + group.getId() + ")";
            PreparedStatement statement = connection.prepareStatement(sqlQuery);
            statement.executeUpdate();

            ResultSet resultSet = statement.executeQuery();
            while (resultSet.next()) {
                invitation = new Invitation(resultSet.getInt("id"), resultSet.getInt("invitee_id"), resultSet.getInt("invitor_id"));
            }
            return invitation;
             */

            List<Invitation> InvitationList = new ArrayList<>();
            Invitation invitation = null;
            Group group = null;

            try {
                Statement statement = connection.createStatement();
                statement.executeUpdate("INSERT INTO notelender.invitation(id, invitor_id, invitee_id, group_id) VALUES (" +
                        invitation.getId() + "," + invitation.getInvitorId() + "," + invitation.getInviteeId() + "," + group.getId() + ")");
                try (ResultSet generatedKeys = statement.getGeneratedKeys()) {
                    if (generatedKeys.next()) {
                        Invitation invitationToAdd = new Invitation(generatedKeys.getInt(1), generatedKeys.getInt(2), generatedKeys.getInt(3));
                        InvitationList.add(invitationToAdd);
                        return gson.toJson(InvitationList);
                    } else {
                        throw new SQLException("Creating failed, no ID obtained.");
                    }
                }
            } catch (SQLException e) {
                System.out.println("Connection failure.");
                e.printStackTrace();
            }
            return null;
        }
    }


    @Override
    public String getInvitation(int id)
            throws SQLException {
        String text = "";
        ResultSet resultSet = connection.createStatement().executeQuery
                ("SELECT * FROM sep3.notelendar.invitations WHERE id = " + id);
        while (resultSet.next()) {
            text = resultSet.getString(2);
            System.out.println(text);
        }
        List<String> invitationsList = new ArrayList<>();
        invitationsList.add(text);
        return gson.toJson(invitationsList);
    }

}


