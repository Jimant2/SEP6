package com.movie4u.sep.service;

import com.movie4u.sep.db.MovieRepository;
import com.movie4u.sep.db.TopListMovieRepository;
import com.movie4u.sep.db.entity.TopListMovie;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.http.ResponseEntity;

@SpringBootTest
class UserServiceTest {

    @Autowired
    TopListMovieRepository topListMovieRepository;
    @Autowired
    UserService userService;



    @Test
    void save() {
       /* TopListMovie topListMovie = new TopListMovie();
        topListMovie.setMovieId(35423);
        topListMovie.setPeopleId(12448 );
        topListMovieRepository.save(topListMovie);*/
    }

  /*  @Test
    void findAllByUserId() {
        var topList = topListMovieRepository.findAllByUsername(12448);
        List<Integer> list = new ArrayList<>();
        for (var t: topList.get()) {
            list.add(t.getMovieId());
        }
        System.out.println(movieRepository.findAllById(list).size());
    }*/

/*    @Test
    public void deleteMovie() {

        var result = topListMovieRepository.findByUsernameAndMovieId("username2", 54724);
        topListMovieRepository.delete(result);
        System.out.println(result.getMovieId());
        System.out.println(result.getUsername());
    }*/

}