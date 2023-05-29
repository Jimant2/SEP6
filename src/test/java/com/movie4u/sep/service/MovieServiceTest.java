package com.movie4u.sep.service;

import com.movie4u.sep.db.MovieRepository;
import com.movie4u.sep.db.PeopleRepository;
import com.movie4u.sep.db.StarRepository;
import com.movie4u.sep.models.RatingStatistics;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.util.ArrayList;
import java.util.List;

@SpringBootTest
class MovieServiceTest {
    @Autowired
    private MovieRepository movieRepository;

    @Autowired
    private PeopleRepository peopleRepository;

    @Autowired
    private StarRepository starRepository;

    @Test
    void findAllMovieByStarName() {
        var results = movieRepository.findRatingStatisticsInDecade(2000L, 2010L);
        List<RatingStatistics> statistics = new ArrayList<>();
        for (Object[] result : results) {
            float rating = (float) result[0];
            long count = (long) result[1];
            RatingStatistics ratingStatistics = new RatingStatistics(rating, count);
            statistics.add(ratingStatistics);
        }
    }
}