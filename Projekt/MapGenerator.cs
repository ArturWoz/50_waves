using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    class MapGenerator
    {
       public Province[,] Randommap(int x)
        {
            Province[,] M = new Province[x, x];
            Terrain[,] TM = new Terrain[x, x];
            double[,] H = RandT(x, false);
            double[,] T = RandT(x, false);
            double[,] W = RandT(x, true);
            Terrain[,,] terr = new Terrain[3, 3, 6];
            terr[2, 0, 0] = Terrain.sea; terr[2, 1, 0] = Terrain.sea; ; terr[2, 2, 0] = Terrain.sea;
            terr[1, 0, 0] = Terrain.sea; terr[1, 1, 0] = Terrain.sea; terr[1, 2, 0] = Terrain.sea;
            terr[0, 0, 0] = Terrain.sea; terr[0, 1, 0] = Terrain.sea; terr[0, 2, 0] = Terrain.sea;
            terr[2, 0, 1] = Terrain.sea; terr[2, 1, 1] = Terrain.sea; terr[2, 2, 1] = Terrain.sea;
            terr[1, 0, 1] = Terrain.sea; terr[1, 1, 1] = Terrain.sea; terr[1, 2, 1] = Terrain.sea;
            terr[0, 0, 1] = Terrain.sea; terr[0, 1, 1] = Terrain.sea; terr[0, 2, 1] = Terrain.sea;
            terr[2, 0, 2] = Terrain.plains; terr[2, 1, 2] = Terrain.farmland; terr[2, 2, 2] = Terrain.jungle;
            terr[1, 0, 2] = Terrain.taiga; terr[1, 1, 2] = Terrain.farmland; terr[1, 2, 2] = Terrain.forest;
            terr[0, 0, 2] = Terrain.tundra; terr[0, 1, 2] = Terrain.farmland; terr[0, 2, 2] = Terrain.desert;
            terr[2, 0, 3] = Terrain.plains; terr[2, 1, 3] = Terrain.forest; terr[2, 2, 3] = Terrain.jungle;
            terr[1, 0, 3] = Terrain.taiga; terr[1, 1, 3] = Terrain.plains; terr[1, 2, 3] = Terrain.forest;
            terr[0, 0, 3] = Terrain.tundra; terr[0, 1, 3] = Terrain.plains; terr[0, 2, 3] = Terrain.desert;
            terr[2, 0, 4] = Terrain.hills; terr[2, 1, 4] = Terrain.hills; terr[2, 2, 4] = Terrain.jungle;
            terr[1, 0, 4] = Terrain.taiga; terr[1, 1, 4] = Terrain.hills; terr[1, 2, 4] = Terrain.forest;
            terr[0, 0, 4] = Terrain.tundra; terr[0, 1, 4] = Terrain.plains; terr[0, 2, 4] = Terrain.desert;
            terr[2, 0, 5] = Terrain.mountains; terr[2, 1, 5] = Terrain.mountains; terr[2, 2, 5] = Terrain.mountains;
            terr[1, 0, 5] = Terrain.mountains; terr[1, 1, 5] = Terrain.mountains; terr[1, 2, 5] = Terrain.mountains;
            terr[0, 0, 5] = Terrain.mountains; terr[0, 1, 5] = Terrain.mountains; terr[0, 2, 5] = Terrain.mountains;


            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    int h = (int)(Math.Round(H[i, j]));
                    if (h > 2) h = 2;
                    if (h < 0) { h = 0; }
                    //  System.Diagnostics.Debug.WriteLine(H[i, j]);

                    int t = (int)(Math.Round(T[i, j]));
                    if (t > 2) t = 2;
                    if (t < 0) t = 0;
                    //  System.Diagnostics.Debug.WriteLine(T[i, j]);
                    int w = (int)(Math.Round(W[i, j]));
                    if (w > 5) w = 5;
                    if (w < 0) w = 0;


                    TM[i, j] = terr[h, t, w];
                }
            }

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    M[i, j] = new Province(j * x + i, 0, TM[i, j], false);
                }
            }
            // post processing
            for (int check = 0; check < x; check++)
            {
                for (int check2 = 0; check2 < x; check2++)
                {
                    Province checkedP;
                    Province checkedP2;
                    checkedP = new Province(M[check, check2]);
                    int L, R, U, D;
                    L = check - 1;
                    R = check + 1;
                    U = check2 - 1;
                    D = check2 + 1;
                    if (checkedP.GetTerrain() != Terrain.sea) // post processing that removes isolated biomes and replaces them with dominating neighbor
                    {
                        Terrain terrain_inQuestion = checkedP.GetTerrain();
                        int plains_count, mountains_count, hills_count, forest_count, tundra_count, taiga_count, desert_count, farmland_count, jungle_count;
                        plains_count = 0; mountains_count = 0; hills_count = 0; forest_count = 0; tundra_count = 0; taiga_count = 0; desert_count = 0; farmland_count = 0; jungle_count = 0;
                        int isolation = 0; // if isolation = 4 province has no neighbors of the same type
                        if (L != -1)
                        {
                            checkedP2 = new Province(M[L, check2]);
                            if (checkedP2.GetTerrain() != terrain_inQuestion) isolation++;
                        }
                        if (R < x)
                        {
                            checkedP2 = new Province(M[R, check2]);
                            if (checkedP2.GetTerrain() != terrain_inQuestion) isolation++;
                        }
                        if (U != -1)
                        {
                            checkedP2 = new Province(M[check, U]);
                            if (checkedP2.GetTerrain() != terrain_inQuestion) isolation++;
                        }
                        if (D < x)
                        {
                            checkedP2 = new Province(M[check, D]);
                            if (checkedP2.GetTerrain() != terrain_inQuestion) isolation++;
                        }
                        //System.Diagnostics.Debug.WriteLine(isolation);
                        if (isolation == 4)
                        {
                            Province checkedL = new Province(checkedP);
                            Province checkedR = new Province(checkedP);
                            Province checkedD = new Province(checkedP);
                            Province checkedU = new Province(checkedP);
                            if (L != -1)
                            {
                                checkedL = new Province(M[L, check2]);
                            }
                            if (R < x)
                            {
                                checkedR = new Province(M[R, check2]);
                            }
                            if (U != -1)
                            {
                                checkedU = new Province(M[check, U]);
                            }
                            if (D < x)
                            {
                                checkedD = new Province(M[check, D]);
                            }

                            if (checkedL.GetTerrain() == Terrain.farmland) { farmland_count++; }
                            else if (checkedL.GetTerrain() == Terrain.mountains) mountains_count++;
                            else if (checkedL.GetTerrain() == Terrain.tundra) tundra_count++;
                            else if (checkedL.GetTerrain() == Terrain.taiga) taiga_count++;
                            else if (checkedL.GetTerrain() == Terrain.forest) forest_count++;
                            else if (checkedL.GetTerrain() == Terrain.hills) hills_count++;
                            else if (checkedL.GetTerrain() == Terrain.desert) desert_count++;
                            else if (checkedL.GetTerrain() == Terrain.plains) plains_count++;
                            else if (checkedL.GetTerrain() == Terrain.jungle) jungle_count++;

                            if (checkedR.GetTerrain() == Terrain.farmland) { farmland_count++; }
                            else if (checkedR.GetTerrain() == Terrain.mountains) mountains_count++;
                            else if (checkedR.GetTerrain() == Terrain.tundra) tundra_count++;
                            else if (checkedR.GetTerrain() == Terrain.taiga) taiga_count++;
                            else if (checkedR.GetTerrain() == Terrain.forest) forest_count++;
                            else if (checkedR.GetTerrain() == Terrain.hills) hills_count++;
                            else if (checkedR.GetTerrain() == Terrain.desert) desert_count++;
                            else if (checkedR.GetTerrain() == Terrain.plains) plains_count++;
                            else if (checkedR.GetTerrain() == Terrain.jungle) jungle_count++;

                            if (checkedU.GetTerrain() == Terrain.farmland) { farmland_count++; }
                            else if (checkedU.GetTerrain() == Terrain.mountains) mountains_count++;
                            else if (checkedU.GetTerrain() == Terrain.tundra) tundra_count++;
                            else if (checkedU.GetTerrain() == Terrain.taiga) taiga_count++;
                            else if (checkedU.GetTerrain() == Terrain.forest) forest_count++;
                            else if (checkedU.GetTerrain() == Terrain.hills) hills_count++;
                            else if (checkedU.GetTerrain() == Terrain.desert) desert_count++;
                            else if (checkedU.GetTerrain() == Terrain.plains) plains_count++;
                            else if (checkedU.GetTerrain() == Terrain.jungle) jungle_count++;

                            if (checkedD.GetTerrain() == Terrain.farmland) { farmland_count++; }
                            else if (checkedD.GetTerrain() == Terrain.mountains) mountains_count++;
                            else if (checkedD.GetTerrain() == Terrain.tundra) tundra_count++;
                            else if (checkedD.GetTerrain() == Terrain.taiga) taiga_count++;
                            else if (checkedD.GetTerrain() == Terrain.forest) forest_count++;
                            else if (checkedD.GetTerrain() == Terrain.hills) hills_count++;
                            else if (checkedD.GetTerrain() == Terrain.desert) desert_count++;
                            else if (checkedD.GetTerrain() == Terrain.plains) plains_count++;
                            else if (checkedD.GetTerrain() == Terrain.jungle) jungle_count++;

                            if (mountains_count >= 2) { M[check, check2].SetTerrain(Terrain.mountains); }
                            else if (farmland_count >= 2) M[check, check2].SetTerrain(Terrain.farmland);
                            else if (tundra_count >= 2) M[check, check2].SetTerrain(Terrain.tundra);
                            else if (taiga_count >= 2) M[check, check2].SetTerrain(Terrain.taiga);
                            else if (forest_count >= 2) M[check, check2].SetTerrain(Terrain.forest);
                            else if (hills_count >= 2) M[check, check2].SetTerrain(Terrain.hills);
                            else if (desert_count >= 2) M[check, check2].SetTerrain(Terrain.desert);
                            else if (jungle_count >= 2) M[check, check2].SetTerrain(Terrain.jungle);
                            else if (plains_count >= 2) M[check, check2].SetTerrain(Terrain.plains);

                        }
                        // post processing part 2, if water is neighbouring, replace province with coast
                        if (checkedP.GetTerrain() != Terrain.sea)
                        {
                            if (L != -1)
                            {
                                checkedP2 = new Province(M[L, check2]);
                                if (checkedP2.GetTerrain() == Terrain.sea) M[check, check2].SetTerrain(Terrain.coast);
                            }
                            if (R < x)
                            {
                                checkedP2 = new Province(M[R, check2]);
                                if (checkedP2.GetTerrain() == Terrain.sea) M[check, check2].SetTerrain(Terrain.coast);
                            }
                            if (U != -1)
                            {
                                checkedP2 = new Province(M[check, U]);
                                if (checkedP2.GetTerrain() == Terrain.sea) M[check, check2].SetTerrain(Terrain.coast);
                            }
                            if (D < x)
                            {
                                checkedP2 = new Province(M[check, D]);
                                if (checkedP2.GetTerrain() == Terrain.sea) M[check, check2].SetTerrain(Terrain.coast);
                            }
                        }
                        isolation = 0;
                        mountains_count = 0;
                        plains_count = 0;
                        desert_count = 0;
                        tundra_count = 0;
                        taiga_count = 0;
                        forest_count = 0;
                        hills_count = 0;
                        farmland_count = 0;
                        jungle_count = 0;
                    }

                }

            }


            return M;
        }
        double[,] RandT(int n, bool is_height)
        {

            int pn = (int)Math.Sqrt(n)/3 * n;
            double[,] O = new double[n, n];
            Point[] Pt = new Point[pn];
            Random rnd = new Random();
            int ik = 0;
            if (!is_height)
            {
                for (int j = 0; j < Math.Sqrt(n); j++)
                {
                    for (int k = 0; k < Math.Sqrt(n); k++)
                    {
                        int rand_point = rnd.Next((int)Math.Sqrt(n)/6, (int)Math.Sqrt(n)/4)+1;
                        for (int i = 0; i < rand_point; i++)
                        {
                            Point p1 = new Point();
                            p1.x = rnd.Next((int)(j * Math.Sqrt(n)), (int)((j + 1) * Math.Sqrt(n)));
                            p1.y = rnd.Next((int)(k * Math.Sqrt(n)), (int)((k + 1) * Math.Sqrt(n)));
                            p1.val = rnd.Next(-5, 6) / 2;
                            Pt[ik] = p1;
                            ik++;
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < Math.Sqrt(n); j++)
                {
                    for (int k = 0; k < Math.Sqrt(n); k++)
                    {
                        int rand_point = rnd.Next((int)Math.Sqrt(n) / 6, (int)Math.Sqrt(n) / 4)+1;
                        for (int i = 0; i < rand_point; i++)
                        {
                            Point p1 = new Point();
                            p1.x = rnd.Next((int)(j * Math.Sqrt(n)), (int)((j + 1) * Math.Sqrt(n)));
                            p1.y = rnd.Next((int)(k * Math.Sqrt(n)), (int)((k + 1) * Math.Sqrt(n)));
                            if (j == 0 || k == 0 || (j == Math.Sqrt(n) - 1) || k == Math.Sqrt(n) - 1) p1.val = -1.25; // change to -3 if you want OG settings
                            else p1.val = rnd.Next(-15, 16)/3; // change to -3, 5 if you want OG settings
                            Pt[ik] = p1;
                            ik++;
                        }
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (is_height) O[i, j] = 2;
                    else O[i, j] = 1;
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < ik; k++)
                    {
                        double l = Math.Sqrt(((i - Pt[k].x) * (i - Pt[k].x)) + ((j - Pt[k].y) * (j - Pt[k].y)));
                        if (l < 1) l = 1;
                        if (!is_height) if (l <= Math.Sqrt(n)) O[i, j] += Pt[k].val / l;
                        if (is_height) { if (l <= Math.Sqrt(n)-2) O[i, j] += Pt[k].val / l; }

                    }
                }
            }
            return O;
        }
        struct Point
        { public int x; public int y; public double val; }
    }
}
